using UnityEngine;
using System.Collections;

public class ActiveLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
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

