using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using UI;
using System;
using Codice.LogWrapper;

namespace Data
{
public class LedgerData : StaticInstance<LedgerData>
{
	public List<LedgerImage> ledgerImages { get; set; } = new List<LedgerImage>();
    public bool isLedgerCreated {get; set;} = false;

    public bool isLeft {get; set;} = false;

    public float flipPageTime {get; set;} = 1f;

    public int pageObjectsIndex {get; set;} = 0;

    public Animator leftHandAnim {get; set;} 
    public Animator rightHandAnim {get; set;}

    public Material drawingPageMaterial {get; set;}

    public GameObject leftHandObj {get; set;}
    public GameObject rightHandObj {get; set;}

    public PencilSketchPostEffect pencilSketchPostEffect {get; set;}
    public PencilSketchPostEffect pencilSketchPostEffectScreenShot {get; set;}
        public Action<LedgerManager> writeActionLedgerManager =  lm => {
        //TODO change to hand state, not function 
         PageAnimations.INSTANCE.DrawImageOnCurrentPage();
         GameEventManager.INSTANCE.OnEvent(typeof(WriteHandState));
         
         
    }; 
    public Action<LedgerMovement> writeActionLedgerMovement =  lm => {
        PageAnimations.INSTANCE.DrawImageOnCurrentPage();
        GameEventManager.INSTANCE.OnEvent(typeof(WriteHandState));
        Debug.Log("write!");
    };
    public Action<LedgerMovement> pointActionLedgerMovement = lm =>{
        if(!LedgerMovement.INSTANCE.IsFlipPageCoroutineRunning())
        {
            GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
         Debug.Log("point!");
        }
    };
     public Action<LedgerManager> pointActionLedgerManager = lm => {
        if(!LedgerMovement.INSTANCE.IsFlipPageCoroutineRunning())
        {
        GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
        }
    };
   
    public Action<PageAnimations> disableleftHandPage = pa =>{
          Debug.LogError("disable left hand page has not been set :/ ");
    };

    public Action<LedgerManager> runClueDialogueOnSelectPage;
    

    public override void OnEnable()
    {
        runClueDialogueOnSelectPage = lm => {

        if(LedgerImageManager.INSTANCE.IsIndexInLedgerImageListRange(pageObjectsIndex))
        {
           int currentClueBodyId = LedgerImageManager.INSTANCE.GetClueBodyIDFromPage(pageObjectsIndex);
           int currentCludId = LedgerImageManager.INSTANCE.GetClueQuestionIDFromPage(pageObjectsIndex);
           DialogueData.INSTANCE.currentCharacterID = currentClueBodyId;
           DialogueData.INSTANCE.currentQuestionID = currentCludId;
     
           GameEventManager.INSTANCE.OnEvent(typeof(ClueConversationState));
           GameEventManager.INSTANCE.OnEvent(typeof(InspectClueLedgerState));
        }

        };

        disableleftHandPage = pa => {
          HandAnimations.INSTANCE.DisableLeftHandPage();
          PageAnimations.onAfterEraseImage.RemoveAction(disableleftHandPage);
          HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.LLastFlip, 1);
        };

        pencilSketchPostEffect = FindObjectsOfType<PencilSketchPostEffect>()[1].GetComponent<PencilSketchPostEffect>();
        pencilSketchPostEffectScreenShot = FindObjectsOfType<PencilSketchPostEffect>()[0].GetComponent<PencilSketchPostEffect>();
        


        DrawingManager.onStartDrawingManager.AddAction(dM => {
            dM.pencilSketchPostEffectScreenShot = pencilSketchPostEffectScreenShot;
            dM.mat = pencilSketchPostEffect.compositeMat;
            
        });


        LedgerManager.onStartLedgerData.AddAction((LedgerManager lm) => {  });
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { ledgerImages = LedgerImageManager.INSTANCE.GetLedgerImageList(); lm.ledgerImages = ledgerImages;  });
        


        LedgerImageManager.onStartLedgerData.AddAction((LedgerImageManager lm) =>{ lm.ledgerImages = ledgerImages; lm.MaxLedgerImageLength = 15; /*change this to a proper variable*/ });
        
        
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { lm.isLedgerCreated = isLedgerCreated;});

        UI.LedgerUIManager.onFlipPage.AddAction((LedgerUIManager luim) => {  luim.flipPageSpeed = this.flipPageTime; luim.isLeft = this.isLeft; });
        LedgerUIManager.onBorderCheck.AddAction((LedgerUIManager ledgerManagerUI) => { ledgerManagerUI.isLeft = this.isLeft; });
        
        PageAnimations.onDrawImageOnCurrentPage.AddAction(lia => {
            lia.currentPageOverlayImage = LedgerUIManager.INSTANCE.GetPageOverlayRenderer(pageObjectsIndex);
        });

        
        LedgerMovement.onEnableHand.AddAction((LedgerMovement ledgerAnimationsManager) => {
            
            ledgerAnimationsManager.flipPageAnimationTime = this.flipPageTime; 
            ledgerAnimationsManager.isLeft = this.isLeft; 
        
        } );

       
        LedgerMovement.onPointHand.AddAction((LedgerMovement ledgerAnimationsManager) => {
        
            ledgerAnimationsManager.isLeft = this.isLeft;
            ledgerAnimationsManager.pageObjectIndex = this.pageObjectsIndex;
            ledgerAnimationsManager.flipPageAnimationTime = flipPageTime;
            
            HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.PointAnim, flipPageTime * 0.5f);
        });


             LedgerMovement.onMove.AddAction((LedgerMovement ledgerAnimationsManager) => {
            
            ledgerAnimationsManager.isLeft = this.isLeft;
            ledgerAnimationsManager.pageObjectIndex = this.pageObjectsIndex;
             ledgerAnimationsManager.flipPageAnimationTime = flipPageTime;
            if(flipPageTime >= 1)
            {
            HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.FlipAnim, flipPageTime);
            }
            else
            {
             HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.FlipAnim, flipPageTime * 10);
            }
        
        });

        LedgerMovement.onWritingHand.AddAction((LedgerMovement lm) => {
             lm.flipPageAnimationTime = flipPageTime;
             lm.pageObjectIndex = this.pageObjectsIndex;
             HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.WriteAnim, flipPageTime);
           
        });

        LedgerMovement.onAfterFlipAwait.AddAction(
        pointActionLedgerMovement
        );
        LedgerManager.onMovePageLeft.AddAction(
        pointActionLedgerManager
        );
        LedgerManager.onMovePageRight.AddAction(
        pointActionLedgerManager
        );
        LedgerMovement.onAfterWritingHand.AddAction((LedgerMovement lm)=>{
         GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState)); //after writing animation is done, we return to new states
         GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
        }
        );
        LedgerMovement.onAfterWritingHand.AddAction(
        pointActionLedgerMovement //after writing animation is done, we return to new states
        );

        base.OnEnable();
    }

}
}

