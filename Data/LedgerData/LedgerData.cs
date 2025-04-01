using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using UI;

namespace Data
{
public class LedgerData : StaticInstance<LedgerData>
{
	public List<LedgerImage> ledgerImages { get; set; } = new List<LedgerImage>();
    public bool isLedgerCreated {get; set;} = true;

    public float flipPageSpeed {get; set;}

    public override void OnEnable()
    {
        LedgerManager.onStartLedgerData.AddAction((LedgerManager lm) => { lm.ledgerImages = ledgerImages; });
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { lm.ledgerImages = ledgerImages; });
        LedgerManager.onActiveLedger.AddAction((LedgerManager lm) => { lm.isLedgerCreated = isLedgerCreated;});
        UI.LedgerUIManager.onFlipPage.AddAction((LedgerUIManager luim) => {  luim.flipPageSpeed = this.flipPageSpeed; });
        base.OnEnable();
    }

}
}

