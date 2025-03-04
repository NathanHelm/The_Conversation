using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ImmediateConversationState : DialogueState
{
    /*conversation is triggered right away!*/
    public override void OnEnter(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
           DialogueManager.INSTANCE.StartConversation(DialogueData.INSTANCE);
           DialogueManager.INSTANCE.RunDialogueNoInput();
        }
    }
    public override void OnUpdate(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
            DialogueManager.INSTANCE.RunDialog();
        }
    }
}
