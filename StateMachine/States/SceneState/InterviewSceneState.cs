using Data;

public class InterviewSceneState : SceneState{
    public override void OnEnter(SceneData data)
    {
       
       //1 -- determine what get loaded (and what doesn't!)
       SavePersistenceManager.INSTANCE.Load();

        //2 -- change state
      // GameEventManager.INSTANCE.OnEvent(typeof(ClueConversationState));

     


    }
    public override void OnExit(SceneData data)
    {
        
        base.OnExit(data);
    }
}