using UnityEngine;
using System.Collections;
using Data;

public class DisableLedgerState : LedgerState
{
    public override void OnEnter(Data.LedgerData data)
    {
        Debug.Log("entered disable ledger state");

        LedgerManager.INSTANCE.DisableLedger();

       

       
        base.OnEnter(data);
    }
    public override void OnUpdate(Data.LedgerData data)
    {
      LedgerManager.INSTANCE.UseLedgerState(); //NOTE THIS WILL CHANGE DEPENDING ON A ID 2 is in trigger or not.
    }
    public override void OnExit(LedgerData data)
    {
     
    }
}

