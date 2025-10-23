using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterviewPP : MonoBehaviour
{
  [SerializeField]
  private Shader postShader;
  [SerializeField]
  private Material postEffectMaterial;
  [SerializeField]

  RenderTexture myRT;
  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
     Graphics.Blit(source, destination, postEffectMaterial);
  }
  private void Start()
  {
   
  }
    
}
