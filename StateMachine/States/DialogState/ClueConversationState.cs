using Data;

public class ClueConversationState : DialogueState{
    //like immediate conversation state, only a few extra step
    //ledger state temporarily become clue-inspection state.
    //image enabled
    //when dialog stops running, you are taken 
    public override void OnEnter(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
            DialogueManager.onEndDialogue.AddAction(DialogueData.INSTANCE.runEndClueInspection); 

            DialogueManager.INSTANCE.StartConversation(data);
            DialogueManager.INSTANCE.RunDialogueNoInput();
        }

    }
    public override void OnUpdate(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
            DialogueManager.INSTANCE.RunDialog();
        }
        base.OnUpdate(data);
    }
    public override void OnExit(DialogueData data)
    {
        DialogueManager.onEndDialogue.RemoveAction(DialogueData.INSTANCE.runEndClueInspection); 

        base.OnExit(data);
    }
}