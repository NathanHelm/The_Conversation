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

    public float flipPageSpeed {get; set;} = 1.0f;

    public override void OnEnable()
    {
        LedgerManager.onStartLedgerData.AddAction((LedgerManager lm) => {  });
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { ledgerImages = LedgerImageManager.INSTANCE.GetLedgerImageList(); lm.ledgerImages = ledgerImages;  });
        
        LedgerImageManager.onStartLedgerData.AddAction((LedgerImageManager lm) =>{ lm.ledgerImages = ledgerImages; lm.MaxLedgerImageLength = 15; /*change this to a proper variable*/ });
        
        
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { lm.isLedgerCreated = isLedgerCreated;});
        UI.LedgerUIManager.onFlipPage.AddAction((LedgerUIManager luim) => {  luim.flipPageSpeed = this.flipPageSpeed; });
        LedgerUIManager.onBorderCheck.AddAction((LedgerUIManager ledgerManagerUI) => { ledgerManagerUI.isLeft = this.isLeft; });
        


    //   LedgerUIManager.INSTANCE.onFlipAt90Degrees.AddAction((LedgerUIManager ledgerManagerUI) => { LedgerManager.INSTANCE.ChangeColorAndLayering(5) ;});

        base.OnEnable();
    }

}
}

