using Data;

public class ClueConversationState : DialogueState{
  
    //when dialog stops running, you are taken back to ledgeropen state
    public override void OnEnter(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
            DialogueManager.onEndDialogue.AddAction(ActionController.INSTANCE.actionEndDialogue.runEndClueInspection); 

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
        DialogueManager.onEndDialogue.RemoveAction(ActionController.INSTANCE.actionEndDialogue.runEndClueInspection); 

        base.OnExit(data);
    }
}