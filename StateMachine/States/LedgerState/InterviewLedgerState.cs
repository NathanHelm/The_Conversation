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

        base.OnEnter(data);

    }
    public override void OnExit(LedgerData data)
    {
        ImageUIAnimations.INSTANCE.EraseInterviewIcon();
    }
    public override void OnUpdate(LedgerData data)
    {
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab))
        {
            GameEventManager.INSTANCE.OnEvent(typeof(InterviewSceneState));
        }
    }
}