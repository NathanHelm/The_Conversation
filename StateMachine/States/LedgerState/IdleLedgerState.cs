using UnityEngine;
using System.Collections;

public class IdleLedgerState : LedgerState
{
    public override void OnEnter(Data.LedgerData data)
    {
        Debug.Log("Idle ledger state.");
    }
}

