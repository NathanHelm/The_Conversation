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
    public bool isLedgerCreated {get; set;} = true;

    public bool isLeft {get; set;} = false;

    public float flipPageSpeed {get; set;} = 1f;

    public int pageObjectsIndex {get; set;} = 0;

    public Animator leftHand; 
    public Animator rightHand;

    public override void OnEnable()
    {
        LedgerManager.onStartLedgerData.AddAction((LedgerManager lm) => {  });
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { ledgerImages = LedgerImageManager.INSTANCE.GetLedgerImageList(); lm.ledgerImages = ledgerImages;  });
        
        LedgerImageManager.onStartLedgerData.AddAction((LedgerImageManager lm) =>{ lm.ledgerImages = ledgerImages; lm.MaxLedgerImageLength = 15; /*change this to a proper variable*/ });
        
        
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { lm.isLedgerCreated = isLedgerCreated;});
        UI.LedgerUIManager.onFlipPage.AddAction((LedgerUIManager luim) => {  luim.flipPageSpeed = this.flipPageSpeed; });
        LedgerUIManager.onBorderCheck.AddAction((LedgerUIManager ledgerManagerUI) => { ledgerManagerUI.isLeft = this.isLeft; });
        
        LedgerMovement.INSTANCE.onEnableHand.AddAction((LedgerMovement ledgerAnimationsManager) => {
            
            ledgerAnimationsManager.flipPageAnimationSpeed = this.flipPageSpeed; 
            ledgerAnimationsManager.isLeft = this.isLeft; 
        
        } );

        
        
       
        LedgerMovement.INSTANCE.onPointHand.AddAction((LedgerMovement LedgerAnimationsManager) => {
        
            HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.PointAnim, flipPageSpeed * 0.5f);
        });


             LedgerMovement.INSTANCE.onMove.AddAction((LedgerMovement ledgerAnimationsManager) => {
            
            ledgerAnimationsManager.isLeft = this.isLeft;
            ledgerAnimationsManager.pageObjectIndex = this.pageObjectsIndex;
            if(flipPageSpeed >= 1)
            {
            HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.FlipAnim, flipPageSpeed);
            }
            else
            {
             HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.FlipAnim, flipPageSpeed * 10);
            }
        
        });
        

        
        base.OnEnable();
    }

}
}

