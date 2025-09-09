using Data;

public class StartSelectionConversationState : ConversationState
{
    //it goes StartSelectionConversationState --> SelectionConversationState
    //all you have to do is press enter!
    public override void OnEnter(DialogueData data)
    {
       
        DialogueManager.INSTANCE.NoDialogue();
        DialogueManager.INSTANCE.PlayerMove();
    }
    public override void OnExit(DialogueData data)
    {
    

    }
    public override void OnUpdate(DialogueData data)
    {
        if (InputBuffer.INSTANCE.IsPressCharacter(UnityEngine.KeyCode.Return))
        {
            //takes you to selection conversation state
            GameEventManager.INSTANCE.OnEvent(typeof(SelectionConversationState));
        }
    }
}