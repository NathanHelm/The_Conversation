using UnityEngine;
using System.Collections;
using Data;
public class StateManager : StaticInstance<StateManager>
{
    /*
      add monobehaviour states here
     */
    public DimensionDataMono dimensionState;
    public PlayerDataMono playerDataState;
    public DialogueStateMono dialogueState;
    public TriggerStateMono triggerState;
    public CutsceneMono cutsceneState;

    /*
     all other state are here
     */
    public TransitionTo2d transitionTo2D = new TransitionTo2d();
    public TransitionTo3d transitionTo3D = new TransitionTo3d();
    public PlayerMove2dState playerMove2DState = new PlayerMove2dState();
    public PlayerLook3dState playerLook3DState = new PlayerLook3dState();
    public PlayerClickOnClueState playerClickOnClueState = new PlayerClickOnClueState();
    public PlayerIdleState playerIdleState = new PlayerIdleState();

    public ConversationState conversationState = new ConversationState();
    public EndConversationState endConversationState = new EndConversationState();
    public NoConversationState noConversationState = new NoConversationState();

    public PlayCutsceneState playCutsceneState = new();
    public StopCutsceneState stopCutsceneState = new();

   // public  custceneState = new 


    public TriggerConversationState triggerConversationState = new TriggerConversationState();
    public DefaultTriggerState defaultTriggerState = new DefaultTriggerState();


    public void Start()
    {
        dimensionState = gameObject.AddComponent<DimensionDataMono>();
        playerDataState = gameObject.AddComponent<PlayerDataMono>();
        dialogueState = gameObject.AddComponent<DialogueStateMono>();
        triggerState = gameObject.AddComponent<TriggerStateMono>();
        cutsceneState = gameObject.AddComponent<CutsceneMono>();


        if (DimensionData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo2d), () => { dimensionState.SwitchState(transitionTo2D); });
            GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo3d), () => { dimensionState.SwitchState(transitionTo3D); });
            dimensionState.SwitchState(transitionTo2D);
        }
        if (PlayerData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(PlayerLook3dState), () => { playerDataState.SwitchState(playerLook3DState); });
            GameEventManager.INSTANCE.AddEvent(typeof(PlayerMove2dState), () => { playerDataState.SwitchState(playerMove2DState); });
            GameEventManager.INSTANCE.AddEvent(typeof(PlayerClickOnClueState), () => { playerDataState.SwitchState(playerClickOnClueState); });
            GameEventManager.INSTANCE.AddEvent(typeof(PlayerIdleState), () => { playerDataState.SwitchState(playerIdleState); });
            playerDataState.SwitchState(playerMove2DState);
        }
        if (DialogueData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(ConversationState), () => { dialogueState.SwitchState(conversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(NoConversationState), () => { dialogueState.SwitchState(noConversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(EndConversationState), () => { dialogueState.SwitchState(endConversationState); });
            dialogueState.SwitchState(endConversationState);
        }
        if (TriggerData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(TriggerConversationState), () => { triggerState.SwitchState(triggerConversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(DefaultTriggerState), () => { triggerState.SwitchState(defaultTriggerState); });
            triggerState.SwitchState(defaultTriggerState);
        }
        if(CutsceneData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(PlayCutsceneState), () => { cutsceneState.SwitchState(playCutsceneState); });
        }

        
        
       
       
       
    }
}

