using UnityEngine;
using System.Collections;
using Data;
public class OpenLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
        Debug.Log("Active ledger state");
        LedgerManager.INSTANCE.OpenLedger();
      
        base.OnEnter(data);
    }
    public override void OnUpdate(LedgerData data)
    {
        LedgerManager.INSTANCE.MovePages();
        
        if(Input.GetKeyDown(KeyCode.Tab))
        {
        GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
        GameEventManager.INSTANCE.OnEvent(typeof(PlayCutsceneState));
        GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));
        }
        
        
    }
    public override void OnExit(LedgerData data)
    {
        base.OnExit(data);
    }
   
}

