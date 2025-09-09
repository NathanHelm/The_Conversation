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
public class ImageUIAnimations : StaticInstance<ImageUIAnimations>, IExecution, IObserver<ObserverAction.LedgerMovementActions>
{

    public static SystemActionCall<ImageUIAnimations> onDrawImageOnCurrentPage = new SystemActionCall<ImageUIAnimations>();
    public static SystemActionCall<ImageUIAnimations> onAfterEraseImage = new SystemActionCall<ImageUIAnimations>();
   
    [SerializeField]
    private PageScriptableObject pageScriptableObject;

    private Texture[] noiseTextures;

    public Renderer currentPageOverlayImage {get; set;}

    private IEnumerator single;

    private List<(Renderer, IEnumerator)> renderersInCoroutine;

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
        DrawImage(currentPageOverlayImage.material);
        
    }


    public void DrawImage(Material material)
    {
        if (single != null)
        {
            StopCoroutine(single);
        }
            StartCoroutine(single = FadeInImageAnimation(1,LedgerData.INSTANCE.flipPageTime * 1.9f, material));
        
    }

    public void EraseImage(Material material)
    {
        if (single != null)
        {
            StopCoroutine(single);
        }
            StartCoroutine(single = FadeOutImageAnimation(0,LedgerData.INSTANCE.flipPageTime * 0.5f, material));
        
    }
    public void FadeInIcon(Material material, float max)
    {
        StartCoroutine(FadeInImageAnimation(max, 0.2f, material));
    }
    public void FadeOutIcon(Material material, float min)
    {
        StartCoroutine(FadeOutImageAnimation(min, 0.2f, material));
    }
    
    

    public IEnumerator FadeInImageAnimation(float maxAlpha, float seconds, Material material)
    {
        Texture nTex = GetRandomNoiseTexture();
        float time = 0;
        var drawShaderMaterial = material;
        drawShaderMaterial.SetTexture("_NoiseTex", nTex);
        while (time < maxAlpha)
        {

            time += Time.deltaTime / seconds;

            drawShaderMaterial.SetFloat("_Val", time);

            yield return new WaitForFixedUpdate();
        }
        drawShaderMaterial.SetFloat("_Val", maxAlpha);
        single = null;
        yield return null;
    }
    public IEnumerator FadeOutImageAnimation(float minAlpha, float seconds, Material material)
    {
      //TODO play around with verticies? 
     
        var drawShaderMaterial = material;
        float time = drawShaderMaterial.GetFloat("_Val"); //1
        while (time > minAlpha)
        {

            time -= Time.deltaTime / seconds;

            drawShaderMaterial.SetFloat("_Val", time);

            yield return new WaitForFixedUpdate();
        }
      drawShaderMaterial.SetFloat("_Val", minAlpha);
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