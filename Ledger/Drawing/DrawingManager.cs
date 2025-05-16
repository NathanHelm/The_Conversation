using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Codice.CM.Client.Differences.Merge;
using UnityEngine;
using UnityEngine.PlayerLoop;
public class DrawingManager : StaticInstance<DrawingManager>{

   public static SystemActionCall<DrawingManager> onStartDrawingManager = new SystemActionCall<DrawingManager>();
   public Material mat {get; set;}
   private Texture2D pencilSketchImage;

   public PencilSketchPostEffect pencilSketchPostEffectScreenShot {get; set;}

    public override void OnEnable()
    {
      MManager.onStartManagersAction.AddAction(m => m.drawingManager = this);
        base.OnEnable();
    }

    public override void m_Start()
    {
        onStartDrawingManager.RunAction(this);
    }
   public void RunDrawingPPEffect()
   {
        Debug.Log("drawing function!");
         if(mat.GetFloat("_T") > 0)
         {
            SetPPToCrossHatch(); 
         }
        StartCoroutine(PostProcessingTransition(1f));
   }
   public Texture2D TakeScreenShot()
   {
      //we are using the pencil sketch to take a screenshot of screen.
      if(mat.GetFloat("_T") > 0)
      {
          SetPPToCrossHatch(); 
          
      }
      return pencilSketchPostEffectScreenShot.TakeScreenShot();
   }
   public IEnumerator PostProcessingTransition(float time)
   {
      float v = 0;
      while(v < 1)
      {
         v += Time.deltaTime / time;
         mat.SetFloat("_T", v);
         yield return new FixedUpdate();
      }
        mat.SetFloat("_T", 1);
        yield return null;
   }
   public void SetPPToCrossHatch()
   {
       mat.SetFloat("_T", 0);
   }



}