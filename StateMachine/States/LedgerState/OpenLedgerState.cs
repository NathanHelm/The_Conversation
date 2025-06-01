using UnityEngine;
using System.Collections;
using Data;
public class OpenLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
        Debug.Log("Active ledger state");
        LedgerManager.INSTANCE.OpenLedger();
        GameEventManager.INSTANCE.OnEvent(typeof(EnableHandState));
      
        base.OnEnter(data);
    }
    public override void OnUpdate(LedgerData data)
    {
        LedgerManager.INSTANCE.MovePages();
        LedgerManager.INSTANCE.SelectPage();
        
        if(Input.GetKeyDown(KeyCode.Tab))
        {
        GameEventManager.INSTANCE.OnEvent(typeof(StopCutsceneState));
        GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
        GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));;                                       
        }
      
        
        
    }
    public override void OnExit(LedgerData data)
    {
        base.OnExit(data);
    }
   
}

