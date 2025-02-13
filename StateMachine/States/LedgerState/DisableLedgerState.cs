using UnityEngine;
using System.Collections;

public class DisableLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
        Debug.Log("entered disable ledger state");
        base.OnEnter(data);
    }
    public override void OnUpdate(LedgerData data)
    {
        LedgerManager.INSTANCE.UseLedger();
    }
}

