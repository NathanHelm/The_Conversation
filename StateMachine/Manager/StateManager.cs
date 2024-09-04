using UnityEngine;
using System.Collections;

public class StateManager : StaticInstance<StateManager>
{
    public DimensionDataMono dimensionState;
    public PlayerDataMono playerDataState;
    public DialogueStateMono dialogueState;



    public TransitionTo2d transitionTo2D = new TransitionTo2d();
    public TransitionTo3d transitionTo3D = new TransitionTo3d();
    public PlayerMove2dState playerMove2DState = new PlayerMove2dState();
    public PlayerLook3dState playerLook3DState = new PlayerLook3dState();
    public ConversationState conversationState = new ConversationState();
    public EndConversationState endConversationState = new EndConversationState();


    public void Start()
    {
        dimensionState = gameObject.AddComponent<DimensionDataMono>();
        playerDataState = gameObject.AddComponent<PlayerDataMono>();
        dialogueState = gameObject.AddComponent<DialogueStateMono>();

        GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo2d), () => { dimensionState.SwitchState(transitionTo2D); });
        GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo3d), () => { dimensionState.SwitchState(transitionTo3D); });
        GameEventManager.INSTANCE.AddEvent(typeof(PlayerLook3dState), () => { playerDataState.SwitchState(playerLook3DState); });
        GameEventManager.INSTANCE.AddEvent(typeof(PlayerMove2dState), () => { playerDataState.SwitchState(playerMove2DState); });

        GameEventManager.INSTANCE.AddEvent(typeof(ConversationState), () => { dialogueState.SwitchState(conversationState); });
        GameEventManager.INSTANCE.AddEvent(typeof(EndConversationState), () => { dialogueState.SwitchState(endConversationState); });
     

        playerDataState.SwitchState(playerMove2DState);
        dimensionState.SwitchState(transitionTo2D);
    }
}

