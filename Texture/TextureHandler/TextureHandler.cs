
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Codice.Client.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class TextureHandler : StaticInstance<TextureHandler>, IExecution
{
    //all in one handler for textures. 
    //can create and access textures in project settings. 
    private readonly string texturePath = "Images/TFILES";
    private readonly string renderTexturePath = "Images/RTFILES";
    private readonly string resourcePath = "Assets/Resources";

    public string GetTexPath() //getting path of texture file
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath,texturePath);
        return path;
    }
    public string GetRTPath()
    {
        string assetsDataPath = Application.dataPath;
        string path = System.IO.Path.Combine(assetsDataPath, renderTexturePath);
        return path;
    }
    public string GetTexRelativePath() //getting path of texture file
    {
        return texturePath;
    }
    public string GetRTRelativePath()
    {
        return renderTexturePath;
    }

    public Texture2D GetTextureRelative(string fileName)
    {
        string assetPath = Path.Combine(GetTexRelativePath(), fileName);
        Texture2D val = Resources.Load<Texture2D>(assetPath);
        Debug.Log("my object " + val);
        return val;
    }
    public Texture2D GetTextureAbsolute(string fileName)
    {
       
        string tex = GetTexPath();
        string assetPath = Path.Combine(tex, fileName);
        byte[] bytes = System.IO.File.ReadAllBytes(assetPath + ".png");
        Texture2D texture = new Texture2D(2, 2); // Initial size, will be resized by LoadImage
        texture.LoadImage(bytes);
       
        return texture;
    }

    public RenderTexture CreateRenderTexture(int width, int height, int depth)
    {
        var renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        renderTexture.depthStencilFormat = GraphicsFormat.D24_UNorm_S8_UInt;
        renderTexture.Create();
        return renderTexture;
    }

    public Texture2DArray FillTexture2DArray(RenderTexture[] renderTextures)
    {
        if (renderTextures.Length == 0)
        {
            return null;
        }

        int width = renderTextures[0].width;
        int height = renderTextures[0].height;
        Texture2DArray textureArray = new Texture2DArray(width, height, renderTextures.Length, TextureFormat.RGBA32, false);
        
        for (int i = 0; i < renderTextures.Length; i++)
        {
        RenderTexture.active = renderTextures[i];
        Texture2D tempTex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tempTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tempTex.Apply();
        textureArray.SetPixels(tempTex.GetPixels(), i);
        }

        textureArray.Apply();
        return textureArray;
    }


    public void SaveRenderTextureToPNG(RenderTexture rt, string fileName)
    {
        StartCoroutine(ConvertRenderTextureToTexture2D(rt, fileName));
    }
    private IEnumerator ConvertRenderTextureToTexture2D(RenderTexture rt, string fileName)
    {
        yield return new WaitForEndOfFrame();
        
        var current = rt;
        RenderTexture.active = current;
        // Create a new Texture2D and read the RenderTexture into it
        Texture2D image = new Texture2D(current.width, current.height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, current.width, current.height), 0, 0);
        image.Apply();
        // Encode texture into PNG
        byte[] bytes = image.EncodeToPNG();
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(GetTexPath(), fileName + ".png"), bytes);
        yield return null;
    }
}