using UnityEngine;
using Data;
public class EndConversationState: DialogueState
{
    public override void OnEnter(DialogueData data)
    {
        
    }
    public override void OnUpdate(DialogueData data)
    {
        data.dialogueManager.RunDialogAgain();
    }

}