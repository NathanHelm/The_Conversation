using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Codice.CM.Client.Differences.Merge;
using UnityEngine;
using UnityEngine.PlayerLoop;
/*


*/
public class DrawingManager : StaticInstance<DrawingManager>, IExecution
{

   public static SystemActionCall<DrawingManager> onStartDrawingManager = new SystemActionCall<DrawingManager>();
   public Material mat { get; set; }
   private Texture2D pencilSketchImage;

   public PencilSketchPostEffect pencilSketchPostEffectScreenShot { get; set; }

   public override void m_OnEnable()
   {
      MManager.INSTANCE.onStartManagersAction.AddAction(m => m.drawingManager = this);
      base.m_OnEnable();
   }


   //1. spawn cam on draw
   //2. 

  


}