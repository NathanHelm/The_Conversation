using UnityEngine;
using System.Collections;

public class DisableLedgerState : LedgerState
{
    public override void OnEnter(Data.LedgerData data)
    {
        Debug.Log("entered disable ledger state");
        base.OnEnter(data);
    }
    public override void OnUpdate(Data.LedgerData data)
    {
        LedgerManager.INSTANCE.UseLedger();
    }
}

