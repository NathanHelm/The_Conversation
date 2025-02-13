using UnityEngine;
using System.Collections;
using Data;

public class NoTriggerState : TriggerState
{
    public override void OnEnter(TriggerData data)
    {
        Debug.Log("trigger idle state");

        TriggerManager.INSTANCE.ChangeTriggerStates(TriggerManager.INSTANCE.IdleTriggerEnterState);
        TriggerManager.INSTANCE.ChangeTriggerStates(TriggerManager.INSTANCE.IdleTriggerEnterState);

        base.OnEnter(data);

    }
}

