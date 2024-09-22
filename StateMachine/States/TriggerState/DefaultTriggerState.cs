using UnityEngine;
using System.Collections;
using Data;

public class DefaultTriggerState: TriggerState
{
    public override void OnEnter(TriggerData data) //default state runs on start() 
    {
        data.triggerManager.SetUpTrigger();
        //changes triggers to default trigger state
        data.triggerManager.ChangeTriggerStates(data.triggerManager.DefaultTrigger);
        data.triggerManager.ChangeTriggerExitState(data.triggerManager.DefaultTriggerExit);
           
    }
    
}

