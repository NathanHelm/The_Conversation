using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using ObserverAction;
using UI;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
public enum LedgerAnimation{
    DrawImage
}
public class PageAnimations : StaticInstance<PageAnimations>, IExecution, IObserver<ObserverAction.LedgerMovementActions>
{

    public static SystemActionCall<PageAnimations> onDrawImageOnCurrentPage = new SystemActionCall<PageAnimations>();
    public static SystemActionCall<PageAnimations> onAfterEraseImage = new SystemActionCall<PageAnimations>();
   
    [SerializeField]
    private PageScriptableObject pageScriptableObject;

    private Texture[] noiseTextures;

    public Renderer currentPageOverlayImage {get; set;}

    private IEnumerator single;

    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction(m => m.pageAnimations = this);
        base.m_OnEnable();
    }

    public override void m_Start()
    {
        noiseTextures = pageScriptableObject.noiseTextures;

    }
    public void DrawImageOnCurrentPage()
    {
        onDrawImageOnCurrentPage.RunAction(this);
        if(currentPageOverlayImage == null)
        {
            Debug.Log("page renderer is not available");
        }
        DrawImage(currentPageOverlayImage);
        
    }



    public void DrawImage(Renderer renderer)
    {
        if (single != null)
        {
            StopCoroutine(single);
        }
            StartCoroutine(single = DrawImageAnimations(LedgerData.INSTANCE.flipPageTime * 1.9f, renderer));
        
    }

    public void EraseImage(Renderer renderer)
    {
        if (single != null)
        {
            StopCoroutine(single);
        }
            StartCoroutine(single = EraseImageAnimations(LedgerData.INSTANCE.flipPageTime * 0.5f, renderer));
        
    }
    
    

    public IEnumerator DrawImageAnimations(float seconds, Renderer renderer)
    {
        Texture nTex = GetRandomNoiseTexture();
        float time = 0;
        var drawShaderMaterial = renderer.material;
        drawShaderMaterial.SetTexture("_NoiseTex", nTex);
        while (time < 1) {

            time += Time.deltaTime / seconds;

            drawShaderMaterial.SetFloat("_Val", time);

            yield return new WaitForFixedUpdate();
        }
        drawShaderMaterial.SetFloat("_Val", 1);
        single = null;
        yield return null;
    }
    public IEnumerator EraseImageAnimations(float seconds, Renderer renderer)
    {
      //TODO play around with verticies? 
      float time = 0;
      var drawShaderMaterial = renderer.material;

      while(time < 1){

      time += Time.deltaTime / seconds;
      
      drawShaderMaterial.SetFloat("_Val", 1 - time);

      yield return new WaitForFixedUpdate();
      }
      drawShaderMaterial.SetFloat("_Val", 0);
      onAfterEraseImage.RunAction(this);
        single = null;
      yield return null;
    }
    private Texture GetRandomNoiseTexture()
    {
        System.Random rnd = new System.Random();
        int i = rnd.Next(0,noiseTextures.Length - 1);
        return noiseTextures[i];
    }

    public void OnNotify(LedgerMovementActions data)
    {
        //for ledgermovement...
        /*
        if (data == LedgerMovementActions.onWriteHand)
        {
            DrawImageOnCurrentPage();
            GameEventManager.INSTANCE.OnEvent(typeof(WriteHandState));
        }
        */
        

    }
}