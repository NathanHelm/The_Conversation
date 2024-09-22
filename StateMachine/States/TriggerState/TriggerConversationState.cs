using UnityEngine;
using System.Collections;
using Data;

public class TriggerConversationState : TriggerState
{
    public override void OnEnter(TriggerData data) //
    {
        //camera positions to the head
        //todo add code that related to the conversation trigger state. 
        GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));
    }

}

