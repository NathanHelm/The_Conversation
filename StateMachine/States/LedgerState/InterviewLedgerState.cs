using System.Diagnostics;
using Data;
using UI;
using UnityEngine;

public class InterviewLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
        LedgerManager.INSTANCE.DisableLedger();
        GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));

        //TODO apply variation
        ImageUIAnimations.INSTANCE.DrawInterviewIcon();

        ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabEnterInterviewScene;

        base.OnEnter(data);

    }
    public override void OnExit(LedgerData data)
    {
        ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabEnterInterviewScene;
        ImageUIAnimations.INSTANCE.EraseInterviewIcon();

    
    }
    public override void OnUpdate(LedgerData data)
    {
        /*
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab))
        {
            ActionController.PRESSTAB(LedgerManager.INSTANCE);
            
        }
        */
    }
}