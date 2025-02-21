using UnityEngine;
using System.Collections;
using Data;
using System;
using System.Collections.Generic;

public class  StateManager: StaticInstance<StateManager>
{
    /*
      add monobehaviour states here
     */
    public DimensionDataMono dimensionState;
    public PlayerDataMono playerDataState;
    public DialogueStateMono dialogueState;
    public TriggerStateMono triggerState;
    public LedgerStateMono ledgerState;
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
    public NoCutsceneState noCutsceneState = new();

    public TriggerConversationState triggerConversationState = new TriggerConversationState();
    public DefaultTriggerState defaultTriggerState = new DefaultTriggerState();
    public NoTriggerState noTriggerState = new NoTriggerState();

    public ActiveLedgerState activeLedgerState = new ActiveLedgerState();
    public IdleLedgerState idleLedgerState = new IdleLedgerState();
    public DisableLedgerState disableLedgerState = new DisableLedgerState();

    public readonly object[] stopStates = new object[] {
        new TransitionTo3d(),
        new NoConversationState(),
        new NoTriggerState(),
        new PlayerIdleState(),
        new IdleLedgerState()
        // CUTSCENE STAYS LAST. If No Cutscene is not last stop state, cutscene will not stop states after it... 
       
    };
    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager e) =>{e.stateManager = this;});
        base.OnEnable();
    }

    public override void m_Start()
    {
        Debug.Log("start is going!!!!! ");
        dimensionState = gameObject.AddComponent<DimensionDataMono>();
        playerDataState = gameObject.AddComponent<PlayerDataMono>();
        dialogueState = gameObject.AddComponent<DialogueStateMono>();
        triggerState = gameObject.AddComponent<TriggerStateMono>();
        cutsceneState = gameObject.AddComponent<CutsceneMono>();
        ledgerState = gameObject.AddComponent<LedgerStateMono>();
        


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
            GameEventManager.INSTANCE.AddEvent(typeof(NoTriggerState), () => { triggerState.SwitchState(noTriggerState); });

            triggerState.SwitchState(defaultTriggerState);
        }
        if (CutsceneData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(PlayCutsceneState), () => { cutsceneState.SwitchState(playCutsceneState); });
            GameEventManager.INSTANCE.AddEvent(typeof(NoCutsceneState), () => {cutsceneState.SwitchState(noCutsceneState); });
            GameEventManager.INSTANCE.AddEvent(typeof(StopCutsceneState), () => { cutsceneState.SwitchState(stopCutsceneState); });
            cutsceneState.SwitchState(noCutsceneState);
        }
        if(LedgerData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(ActiveLedgerState), () => { ledgerState.SwitchState(activeLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(IdleLedgerState), () => { ledgerState.SwitchState(idleLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(DisableLedgerState), () => { ledgerState.SwitchState(disableLedgerState); });
            ledgerState.SwitchState(disableLedgerState);
        }
      
    }

    public Dictionary<string, Type> GetStateHashmap(object[] newStates) //key = root name, val = state that it has
    {
        Dictionary<string, Type> keyValuePairs = new Dictionary<string, Type>();
        
        string[] stateParent = new string[] { "PlayerState", "CutsceneState", "DialogueState", "TriggerState", "DimensionState", "LedgerState" };

        for (int i = 0; i < newStates.Length; i++)
        {
            if (newStates[i] is PlayerState)
            {
                keyValuePairs.Add(stateParent[0], newStates[i].GetType());
            }
            else if (newStates[i] is CutsceneState)
            {
                keyValuePairs.Add(stateParent[1], newStates[i].GetType());
            }
            else if (newStates[i] is DialogueState)
            {
                keyValuePairs.Add(stateParent[2], newStates[i].GetType());
            }
            else if (newStates[i] is TriggerState)
            {
                keyValuePairs.Add(stateParent[3], newStates[i].GetType());
            }
            else if (newStates[i] is DimensionState)
            {
                keyValuePairs.Add(stateParent[4], newStates[i].GetType());
            }
            else if (newStates[i] is LedgerState)
            {
                keyValuePairs.Add(stateParent[5], newStates[i].GetType());
            }
            else
            {
                throw new Exception("state " + newStates[i].ToString() + " has no state");
            }
         
        }
        return keyValuePairs;
    }
    public object[] SnapShotCurrentStates()
    {
        return new object[] { dimensionState.currentState, playerDataState.currentState, dialogueState.currentState, triggerState.currentState, cutsceneState.currentState, ledgerState.currentState} ;
    }
    
}

