using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using UI;
using System;

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

        public Action<LedgerManager> writeActionLedgerManager =  lm => {
        //TODO change to hand state, not function 
         PageAnimations.INSTANCE.DrawImageOnCurrentPage();
         GameEventManager.INSTANCE.OnEvent(typeof(WriteHandState));
         
    }; 
    public Action<LedgerMovement> writeActionLedgerMovement =  lm => {
        PageAnimations.INSTANCE.DrawImageOnCurrentPage();
        GameEventManager.INSTANCE.OnEvent(typeof(WriteHandState));
    };
    public Action<LedgerMovement> pointActionLedgerMovement = lm =>{
        if(!LedgerMovement.INSTANCE.IsFlipPageCoroutineRunning())
        {
        GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
        }
    };
     public Action<LedgerManager> pointActionLedgerManager = lm => {
        if(!LedgerMovement.INSTANCE.IsFlipPageCoroutineRunning())
        {
        GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
        }
    };
    

    public override void OnEnable()
    {




        LedgerManager.onStartLedgerData.AddAction((LedgerManager lm) => {  });
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { ledgerImages = LedgerImageManager.INSTANCE.GetLedgerImageList(); lm.ledgerImages = ledgerImages;  });
        
        LedgerImageManager.onStartLedgerData.AddAction((LedgerImageManager lm) =>{ lm.ledgerImages = ledgerImages; lm.MaxLedgerImageLength = 15; /*change this to a proper variable*/ });
        
        
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { lm.isLedgerCreated = isLedgerCreated;});
        UI.LedgerUIManager.onFlipPage.AddAction((LedgerUIManager luim) => {  luim.flipPageSpeed = this.flipPageTime; });
        LedgerUIManager.onBorderCheck.AddAction((LedgerUIManager ledgerManagerUI) => { ledgerManagerUI.isLeft = this.isLeft; });
        
        PageAnimations.onDrawImageOnCurrentPage.AddAction(lia => {
            lia.currentPageOverlayImage = LedgerUIManager.INSTANCE.GetPageOverlayRenderer(pageObjectsIndex);
        });
        
        LedgerMovement.INSTANCE.onEnableHand.AddAction((LedgerMovement ledgerAnimationsManager) => {
            
            ledgerAnimationsManager.flipPageAnimationTime = this.flipPageTime; 
            ledgerAnimationsManager.isLeft = this.isLeft; 
        
        } );

       
        LedgerMovement.INSTANCE.onPointHand.AddAction((LedgerMovement ledgerAnimationsManager) => {
        
            ledgerAnimationsManager.isLeft = this.isLeft;
            ledgerAnimationsManager.pageObjectIndex = this.pageObjectsIndex;
            ledgerAnimationsManager.flipPageAnimationTime = flipPageTime;
            
            HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.PointAnim, flipPageTime * 0.5f);
        });


             LedgerMovement.INSTANCE.onMove.AddAction((LedgerMovement ledgerAnimationsManager) => {
            
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

        LedgerMovement.INSTANCE.onWritingHand.AddAction((LedgerMovement lm) => {
             lm.flipPageAnimationTime = flipPageTime;
             lm.pageObjectIndex = this.pageObjectsIndex;
             HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.WriteAnim, flipPageTime);
           
        });

        LedgerMovement.INSTANCE.onAfterFlipAwait.AddAction(
        pointActionLedgerMovement
        );
        LedgerManager.onMovePageLeft.AddAction(
        pointActionLedgerManager
        );
        LedgerManager.onMovePageRight.AddAction(
        pointActionLedgerManager
        );
        LedgerMovement.INSTANCE.onAfterWritingHand.AddAction((LedgerMovement lm)=>{
         GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState)); //after writing animation is done, we return to new states
         GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
        }
        );
        LedgerMovement.INSTANCE.onAfterWritingHand.AddAction(
        pointActionLedgerMovement //after writing animation is done, we return to new states
        );

        base.OnEnable();
    }

}
}

