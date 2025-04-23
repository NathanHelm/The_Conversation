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
      if(Input.GetKeyDown(KeyCode.Tab))
      {
          GameEventManager.INSTANCE.OnEvent(typeof(StopCutsceneState));
          GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState));
          GameEventManager.INSTANCE.OnEvent(typeof(EnableHandState));
         
      }
    }
}

