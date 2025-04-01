using UnityEngine;
using Data;
public class EndConversationState: DialogueState
{
    public override void OnEnter(DialogueData data)
    {
        
        DialogueManager.INSTANCE.NoDialogue();
        DialogueManager.INSTANCE.PlayerMove();
       

    }
    public override void OnUpdate(DialogueData data)
    {
        DialogueManager.INSTANCE.RunDialogAgain();
    }

}