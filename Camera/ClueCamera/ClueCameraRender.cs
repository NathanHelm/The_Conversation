using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Data;
using UnityEngine;

public class ClueCameraRender : MonoBehaviour, IExecution, IObserver<LedgerImageManager>
{
    public RenderTexture renderTexture { get; private set; }
    [SerializeField]
    Material sobelFilter;
    private SobelMachine sobelMachine;

    private Camera cam;

    private Material mat;

    public void m_Awake()
    {

    }

    public void m_GameExecute()
    {
       
    }

    public void m_OnEnable()
    {
        throw new System.NotImplementedException();
    }

    public void CreateClueCameraRender(int width, int height, int depth)
    {
        sobelMachine = ClueCameraData.INSTANCE?.sobelMachine;
        cam = GetComponent<Camera>();
        cam.depthTextureMode |= DepthTextureMode.DepthNormals;
        cam.depthTextureMode |= DepthTextureMode.Depth;
        mat = new Material(sobelFilter);
        sobelMachine.SetSobelFilter(ref mat);
        renderTexture = TextureHandler.INSTANCE.CreateRenderTexture(width, height, depth);
        
    }
    

    public void OnNotify(LedgerImageManager data)
    {
        //send data to array...
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        /*here, we are releaseing and*/
        renderTexture.Release();

        renderTexture.Create();


        Graphics.Blit(source, renderTexture, mat);
    }
}
