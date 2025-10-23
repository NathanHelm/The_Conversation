using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraRenderTexture : MonoBehaviour, IExecution
{
    [SerializeField]
    Material mat;
    [SerializeField]
    private RenderTexture renderTexture;
    [SerializeField]
    private Texture returnTex;

    public void m_Awake()
    {
        // throw new System.NotImplementedException();
    }

    public void m_GameExecute()
    {
        //mat.SetTexture("_MainTex",renderTexture); 
    }


    public void m_OnEnable()
    {
        Graphics.Blit(returnTex, renderTexture, mat);
        // throw new System.NotImplementedException();
    }
}
