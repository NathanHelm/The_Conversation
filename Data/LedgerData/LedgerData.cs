using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;

public class LedgerData : StaticInstance<LedgerData>
{
	public List<LedgerImage> ledgerImages { get; set; } = new List<LedgerImage>();


    public override void OnEnable()
    {
        LedgerManager.onStartLedgerData.AddAction((LedgerManager lm) => { lm.ledgerImages = ledgerImages; });
        LedgerManager.onShowLedgerImages.AddAction((LedgerManager lm) => { lm.ledgerImages = ledgerImages; });
        base.m_Start();
    }

}

