using UnityEngine;
using Data;
public class SelectionConversationState : ConversationState
{
    public override void OnEnter(DialogueData data)
    {
        GameEventManager.INSTANCE?.OnEvent(typeof(PlayerIdleState)); //player is idle
        DialogueSelectionManager.INSTANCE.CreateButtons();
        DialogueSelectionManager.INSTANCE.SetTextToName();
        DialogueSelectionManager.INSTANCE.StartSelectionButtonLeft();
    }
    public override void OnExit(DialogueData data)
    {
        
    }
    public override void OnUpdate(DialogueData data)
    {
        DialogueSelectionManager.INSTANCE.RunSelectedDialogue();
    }
}