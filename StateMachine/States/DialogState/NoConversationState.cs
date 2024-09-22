using UnityEngine;
using System.Collections;
using Data;

public class NoConversationState : DialogueState
{
    public override void OnEnter(DialogueData data)
    {
        data.dialogueManager.NoDialogue();
    }
}

