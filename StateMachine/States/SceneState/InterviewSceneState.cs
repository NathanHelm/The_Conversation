using Data;

public class InterviewSceneState : SceneState{
    public override void OnEnter(SceneData data)
    {

        //1 -- determine what get loaded (and what doesn't!)
        // SavePersistenceManager.INSTANCE.Load();
        //  DialogueManager.INSTANCE
        //2 -- change state (when you press return after conversation


        InterviewData.INSTANCE?.Load(); //load dialogue data with proper data...
        CharacterManager.INSTANCE?.Load();

        GameEventManager.INSTANCE?.OnEvent(typeof(OpenLedgerState));
        
        LedgerImageManager.INSTANCE?.Load();
        QuestionResponseManager.INSTANCE?.Load();




        LedgerManager.onSelectPage.AddAction(LedgerData.INSTANCE.runInterviewScene);
        

      

        //after dialogue, you are take 
        //  DialogueManager.onEndDialogue.AddAction(DialogueData.INSTANCE.runEndClueInspection;);




    }
    public override void OnExit(SceneData data)
    {
 
        base.OnExit(data);
    }
}