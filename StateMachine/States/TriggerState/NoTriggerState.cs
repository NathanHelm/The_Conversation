using UnityEngine;
using System.Collections;
using Data;

public class NoTriggerState : TriggerState
{
    public override void OnEnter(TriggerData data)
    {
        Debug.Log("trigger idle state");
        base.OnEnter(data);

    }
}

