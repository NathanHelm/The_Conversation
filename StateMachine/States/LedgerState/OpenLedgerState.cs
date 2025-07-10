using UnityEngine;
using System.Collections;
using Data;
using System;
public class OpenLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
        Debug.Log("Active ledger state");
        GameEventManager.INSTANCE.OnEvent(typeof(EnableHandState));
        
        LedgerManager.INSTANCE.OpenLedger();

        ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopCutscene;
        ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabDisableLedger;
      

        base.OnEnter(data);
    }
    public override void OnUpdate(LedgerData data)
    {

        LedgerManager.INSTANCE.MovePages();

        ActionController.INSTANCE.PressReturn();

        /*
        
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab))
        {
            ActionController.PRESSTAB(LedgerManager.INSTANCE);
        }
        */



    }
    public override void OnExit(LedgerData data)
    {
        ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopCutscene;
        ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabDisableLedger;


        Debug.Log("leaving open ledger! but why?");
        base.OnExit(data);
    }
   
}

