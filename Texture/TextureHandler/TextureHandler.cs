
using System.IO;
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

    public string GetTexPath() //getting path of texture file
    {
        string assetsDataPath = Application.dataPath;
        string path = System.IO.Path.Combine(assetsDataPath, texturePath);
        return path;
    }
    public string GetRTPath()
    {
        string assetsDataPath = Application.dataPath;
        string path = System.IO.Path.Combine(assetsDataPath, renderTexturePath);
        return path;
    }

    public Texture2D GetTexture2D(string fileName)
    {
        string assetPath = Path.Combine(GetRTPath(), fileName);
        return AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
    }
    public RenderTexture CreateRenderTexture(int width, int height, int depth)
    {
        var renderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        renderTexture.depthStencilFormat = GraphicsFormat.D24_UNorm_S8_UInt;
        renderTexture.Create();
        return renderTexture;
    }


    public void SaveRenderTextureToPNG(RenderTexture rt, string fileName)
    {
        // Create a new Texture2D and read the RenderTexture into it
        Texture2D image = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        image.Apply();
        // Encode texture into PNG
        byte[] bytes = image.EncodeToPNG();
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(GetTexPath(), fileName), bytes);
    }


}