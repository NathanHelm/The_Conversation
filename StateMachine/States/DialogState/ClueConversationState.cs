using Data;

public class ClueConversationState : DialogueState{
  
    //when dialog stops running, you are taken back to ledgeropen state
    public override void OnEnter(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {

            ActionController.AFTERDIALOGUE += ActionController.INSTANCE.actionEndDialogue.runEndClueInspection;//run clue!
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
       // DialogueManager.INSTANCE.onEndDialogue.RemoveAction(ActionController.INSTANCE.actionEndDialogue.runEndClueInspection); 
        ActionController.AFTERDIALOGUE -= ActionController.INSTANCE.actionEndDialogue.runEndClueInspection;//run clue!
        
        base.OnExit(data);
    }
}