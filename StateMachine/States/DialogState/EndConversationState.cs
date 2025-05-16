using Data;
using UnityEngine;

public class EndConversationState : DialogueState
{
    public override void OnEnter(DialogueData data)
    {
        DialogueManager.INSTANCE.NoDialogue();

        base.OnEnter(data);
    }
}