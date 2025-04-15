using UnityEngine;
using System.Collections;
using Data;
public class OpenLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
        Debug.Log("Active ledger state");
        LedgerManager.INSTANCE.EnableLedger();
        base.OnEnter(data);
    }
    public override void OnUpdate(LedgerData data)
    {
        LedgerManager.INSTANCE.MovePages();
    }
    public override void OnExit(LedgerData data)
    {
        LedgerManager.INSTANCE.DisableLedger();
        base.OnExit(data);
    }
}

