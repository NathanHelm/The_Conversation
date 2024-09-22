using UnityEngine;
using Data;
public class EndConversationState: DialogueState
{
    public override void OnEnter(DialogueData data)
    {
        data.dialogueManager.NoDialogue();
        
    }
    public override void OnUpdate(DialogueData data)
    {
        data.dialogueManager.RunDialogAgain();
    }

}