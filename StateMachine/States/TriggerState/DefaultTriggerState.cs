using UnityEngine;
using System.Collections;
using Data;

public class DefaultTriggerState: TriggerState
{
    public override void OnEnter(TriggerData data) //default state runs on start() 
    {
       
        //changes triggers to default trigger state
        TriggerManager.INSTANCE.ChangeTriggerStates(TriggerManager.INSTANCE.DefaultTrigger);
        TriggerManager.INSTANCE.ChangeTriggerExitState(TriggerManager.INSTANCE.DefaultTriggerExit);
           
    }
    
}

