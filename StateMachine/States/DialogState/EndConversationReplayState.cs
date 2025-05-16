using UnityEngine;
using Data;
public class EndConversationReplayState: DialogueState
{
    public override void OnEnter(DialogueData data)
    {
        
        DialogueManager.INSTANCE.NoDialogue();
        DialogueManager.INSTANCE.PlayerMove();
       

    }
    public override void OnUpdate(DialogueData data)
    {
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return)) //plays the dialog again
        {
            DialogueManager.INSTANCE.RunDialogAgain();
        }
    }

}