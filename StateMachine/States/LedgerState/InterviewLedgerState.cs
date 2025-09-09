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
        IconUIAnimations.INSTANCE.FadeInIconRenderer(IconUIAnimations.INSTANCE.GetIconRenderer(Icon.enterInterviewSceneIcon));

        ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabEnterInterviewScene;

        base.OnEnter(data);

    }
    public override void OnExit(LedgerData data)
    {
        ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabEnterInterviewScene;
        IconUIAnimations.INSTANCE.FadeOutIconRenderer(IconUIAnimations.INSTANCE.GetIconRenderer(Icon.enterInterviewSceneIcon));

    
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