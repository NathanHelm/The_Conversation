using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PPShader : MonoBehaviour
{
  [SerializeField]
  private Shader postShader;
  [SerializeField]
  private Material postEffectMaterial;
  [SerializeField]

  RenderTexture myRT;
  [Header("for edge detection filter only")]
  [SerializeField]
  SobelMachine sobelMachine;

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    Graphics.Blit(source, destination, postEffectMaterial);
    if (sobelMachine != null)
    {
      sobelMachine.SetSobelFilter(ref postEffectMaterial);
    }
  }
  private void Start()
  {
    var camera = GetComponent<Camera>();
    camera.depthTextureMode |= DepthTextureMode.Depth;
    if (sobelMachine == null)
    {
      Debug.LogError("sobelMachine is null");
    }
  
   
  }
    
}
