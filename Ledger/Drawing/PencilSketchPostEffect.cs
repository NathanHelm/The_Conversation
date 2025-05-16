using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PencilSketchPostEffect : MonoBehaviour
{
    public float bufferScale = 1.0f;
    public Shader uvReplacementShader;
    public Material compositeMat;

    private Camera mainCam;
    private int scaledWidth;
    private int scaledHeight;
    [SerializeField]
    private bool isAddCamera = false;
    
    [SerializeField]
    private Camera effectCamera;
    [SerializeField]
    private Texture2D screenShotTex;

    private readonly string SketchImagePath = "Assets/Shader/TextureOutput/temp.png";

    void Start()
    {
        Application.targetFrameRate = 120;
        mainCam = GetComponent<Camera>();
        mainCam.depthTextureMode |= DepthTextureMode.Depth;
    }

    void Update()
    {
        bufferScale = Mathf.Clamp(bufferScale, 0.0f, 1.0f);
        scaledWidth = (int)(Screen.width * bufferScale);
        scaledHeight = (int)(Screen.height * bufferScale);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if(effectCamera == null)
        {
            return;
        }
        effectCamera.CopyFrom(mainCam);
        effectCamera.transform.position = transform.position;
        effectCamera.transform.rotation = transform.rotation;


        RenderTexture uvBuffer = RenderTexture.GetTemporary(scaledWidth, scaledHeight, 24, RenderTextureFormat.ARGBFloat);
        effectCamera.SetTargetBuffers(uvBuffer.colorBuffer, uvBuffer.depthBuffer);
        effectCamera.RenderWithShader(uvReplacementShader, "");

        compositeMat.SetTexture("_UVBuffer", uvBuffer);

        Graphics.Blit(src, dst, compositeMat);

        RenderTexture.ReleaseTemporary(uvBuffer);
    }
    public Texture2D TakeScreenShot()
    {
    RenderTexture rt = new RenderTexture(scaledWidth, scaledHeight, 24);
    mainCam.targetTexture = rt;
    Texture2D screenshot = new Texture2D(scaledWidth, scaledHeight, TextureFormat.RGB24, false);
    mainCam.Render();
    RenderTexture.active = rt;
    screenshot.ReadPixels(new Rect(0, 0, scaledWidth, scaledHeight), 0, 0);
    screenshot.Apply();
    mainCam.targetTexture = null;
    RenderTexture.active = null;
    Destroy(rt);
    File.WriteAllBytes(SketchImagePath, screenshot.EncodeToPNG());
    return screenshot;
    }
}