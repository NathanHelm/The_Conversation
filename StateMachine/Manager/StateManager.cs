using UnityEngine;
using Data;
using System;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;

public class StateManager: StaticInstance<StateManager>, IExecution
{
    public Subject<ObserverAction.StateMachineAction> subject { get; set; } = new();

    /*
      add monobehaviour states here
     */
    public DimensionDataMono dimensionState;
    public PlayerDataMono playerDataState;
    public DialogueStateMono dialogueState;
    public TriggerStateMono triggerState;
    public LedgerStateMono ledgerState;
    public HandStateMono handState;
    public CutsceneMono cutsceneState;
    public SceneStateMono sceneStateMono;

    /*
     all other state are here
     */
    public TransitionTo2d transitionTo2D = new TransitionTo2d();
    public TransitionTo3d transitionTo3D = new TransitionTo3d();
    public PlayerMove2dState playerMove2DState = new PlayerMove2dState();
    public PlayerLook3dState playerLook3DState = new PlayerLook3dState();
    public Playerlook3dNoCursorWrap playerlook3DNoCursorWrap = new();
    public PlayerClickOnClueState playerClickOnClueState = new PlayerClickOnClueState();
    public PlayerIdleState playerIdleState = new PlayerIdleState();

    public ConversationState conversationState = new ConversationState();
    public EndConversationReplayState endConversationReplayState = new EndConversationReplayState();
    public NoConversationState noConversationState = new NoConversationState();
    public ImmediateConversationState immediateConversationState = new();
    public ButtonOptionDialogState buttonOptionDialogState = new();
    public ClueConversationState clueConversationState = new();
    public EndConversationState endConversationState = new();
    public SelectionConversationState selectionConversationState = new();
    public StartSelectionConversationState startSelectionConversationState = new();

    public PlayCutsceneState playCutsceneState = new();
    public StopCutsceneState stopCutsceneState = new();
    public NoCutsceneState noCutsceneState = new();


    public TriggerConversationState triggerConversationState = new TriggerConversationState();
    public DefaultTriggerState defaultTriggerState = new DefaultTriggerState();
    public NoTriggerState noTriggerState = new NoTriggerState();

    public OpenLedgerState openLedgerState = new OpenLedgerState();
    public IdleLedgerState idleLedgerState = new IdleLedgerState();
    public DisableLedgerState disableLedgerState = new DisableLedgerState();
    public ReplaceLedgerState replaceLedgerState = new();
    public WriteToPageLedgerState writeToPageLedgerState = new();
    public InspectClueLedgerState inspectClueLedgerState = new();
    public InterviewLedgerState interviewLedgerState = new();
    public PreviousSceneInterviewLedgerState previousSceneInterviewLedgerState = new();



    public MoveHandFlipState moveHandFlipState = new();
    public PointHandState pointHandState = new();

    public WriteHandState writeHandState = new();

    public EnableHandState enableHandState = new();
    public DisableHandState disableHandState = new();

    public ClickHandState clickHandState = new();

    public IdleHandState idleHandState = new();

    public HoldPageState holdPageState = new();

    //scene states below========================================================================================
    public VetHouseSceneState vetHouseSceneState = new();
    public SampleSceneState sampleSceneState = new();
    public InterviewSceneState interviewSceneState = new();
    public MemorySceneState memorySceneState = new();
    public IdleSceneState idleSceneState = new();

    //=============================================================================================================

    public readonly object[] stopStates = new object[] {
        new TransitionTo3d(),
        new NoCutsceneState(),
        new NoConversationState(),
        new NoTriggerState(),
        new PlayerIdleState(),
        new IdleLedgerState(),
        new IdleHandState(),
        
    
        // CUTSCENE STAYS LAST. If No Cutscene is not last stop state, cutscene will not stop states after it... 
       
    };
    private readonly string[] stateParent = new string[] { "PlayerState", "CutsceneState", "DialogueState", "TriggerState", "DimensionState", "LedgerState", "HandState" };

    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager e) =>{e.stateManager = this;});
        base.m_OnEnable();
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
        handState = gameObject.AddComponent<HandStateMono>();
        sceneStateMono = gameObject.AddComponent<SceneStateMono>();
        


        if (DimensionData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo2d), () => { dimensionState.SwitchState(transitionTo2D); });
            GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo3d), () => { dimensionState.SwitchState(transitionTo3D); });
            dimensionState.SwitchState(transitionTo2D);
        }
        if (PlayerData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(PlayerLook3dState), () => { playerDataState.SwitchState(playerLook3DState); });
            GameEventManager.INSTANCE.AddEvent(typeof(Playerlook3dNoCursorWrap), () => { playerDataState.SwitchState(playerLook3DState); });

            GameEventManager.INSTANCE.AddEvent(typeof(PlayerMove2dState), () => { playerDataState.SwitchState(playerMove2DState); });
            GameEventManager.INSTANCE.AddEvent(typeof(PlayerClickOnClueState), () => { playerDataState.SwitchState(playerClickOnClueState); });
            GameEventManager.INSTANCE.AddEvent(typeof(PlayerIdleState), () => { playerDataState.SwitchState(playerIdleState); });
            playerDataState.SwitchState(playerMove2DState);
        }
        if (DialogueData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(ConversationState), () => { dialogueState.SwitchState(conversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(NoConversationState), () => { dialogueState.SwitchState(noConversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(EndConversationReplayState), () => { dialogueState.SwitchState(endConversationReplayState); });
            GameEventManager.INSTANCE.AddEvent(typeof(ImmediateConversationState), ()=>{dialogueState.SwitchState(immediateConversationState);});
            GameEventManager.INSTANCE.AddEvent(typeof(ButtonOptionDialogState), ()=>{dialogueState.SwitchState(buttonOptionDialogState);});
            GameEventManager.INSTANCE.AddEvent(typeof(ClueConversationState), ()=> {dialogueState.SwitchState(clueConversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(EndConversationState), () => { dialogueState.SwitchState(endConversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(SelectionConversationState), () => { dialogueState.SwitchState(selectionConversationState); });
            GameEventManager.INSTANCE.AddEvent(typeof(StartSelectionConversationState), () => { dialogueState.SwitchState(startSelectionConversationState); });
           
            dialogueState.SwitchState(noConversationState);
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
        if (LedgerData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(OpenLedgerState), () => { ledgerState.SwitchState(openLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(IdleLedgerState), () => { ledgerState.SwitchState(idleLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(DisableLedgerState), () => { ledgerState.SwitchState(disableLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(WriteToPageLedgerState), () => { ledgerState.SwitchState(writeToPageLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(ReplaceLedgerState), () => { ledgerState.SwitchState(replaceLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(InspectClueLedgerState), () => { ledgerState.SwitchState(inspectClueLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(InterviewLedgerState), () => { ledgerState.SwitchState(interviewLedgerState); });
            GameEventManager.INSTANCE.AddEvent(typeof(PreviousSceneInterviewLedgerState), () => { ledgerState.SwitchState(previousSceneInterviewLedgerState); });
         
            //yes...hand state and ledgerstate carry the same data
            GameEventManager.INSTANCE.AddEvent(typeof(IdleHandState), () => { handState.SwitchState(idleHandState); });
            GameEventManager.INSTANCE.AddEvent(typeof(EnableHandState), () => { handState.SwitchState(enableHandState); });
            GameEventManager.INSTANCE.AddEvent(typeof(DisableHandState), () => { handState.SwitchState(disableHandState); });
            GameEventManager.INSTANCE.AddEvent(typeof(PointHandState), () => { handState.SwitchState(pointHandState); });
            GameEventManager.INSTANCE.AddEvent(typeof(MoveHandFlipState), () => { handState.SwitchState(moveHandFlipState); });
            GameEventManager.INSTANCE.AddEvent(typeof(ClickHandState), () => { handState.SwitchState(clickHandState); });
            GameEventManager.INSTANCE.AddEvent(typeof(WriteHandState), () => { handState.SwitchState(writeHandState); });
            GameEventManager.INSTANCE.AddEvent(typeof(HoldPageState), () => { handState.SwitchState(holdPageState); });


            ledgerState.SwitchState(idleLedgerState);
            handState.SwitchState(idleHandState);
            
        }
        //this comes last-- scene states have to potential to change other states!
        if(SceneData.INSTANCE != null)
        {
            GameEventManager.INSTANCE.AddEvent(typeof(SampleSceneState), ()=> { sceneStateMono.SwitchScene(vetHouseSceneState, SceneNames.SampleScene);});
            GameEventManager.INSTANCE.AddEvent(typeof(InterviewSceneState),()=>{ sceneStateMono.SwitchScene(interviewSceneState, SceneNames.InterviewScene);} );
            GameEventManager.INSTANCE.AddEvent(typeof(MemorySceneState),()=>{ sceneStateMono.SwitchScene(memorySceneState, SceneNames.MemoryScene);} );
            GameEventManager.INSTANCE.AddEvent(typeof(VetHouseSceneState),()=>{ sceneStateMono.SwitchScene(vetHouseSceneState, SceneNames.VetHouseScene);} );
        
            GameEventManager.INSTANCE.AddEvent(typeof(IdleSceneState), ()=> {
            sceneStateMono.SwitchState(idleSceneState);});



            if (SceneData.CURRENTSCENE == SceneNames.None)
            {
                Debug.LogError("Assigning state vet house because current scene is null!");
               // SceneStateMono.currentSceneState = vetHouseSceneState;
                sceneStateMono.SwitchState(vetHouseSceneState);
                //sceneStateMono.SwitchState(idleHandState);
            }
        }
        
      
    }
    /*TODO explain this function below*/
    public Dictionary<string, Type> GetStateHashmap(object[] newStates) //key = root name, val = state that it has
    {
        Dictionary<string, Type> keyValuePairs = new Dictionary<string, Type>();
        
     
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
            else if (newStates[i] is HandState)
            {
                keyValuePairs.Add(stateParent[6], newStates[i].GetType());
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
        return new object[] { dimensionState.currentState, playerDataState.currentState, dialogueState.currentState, triggerState.currentState, cutsceneState.currentState, ledgerState.currentState, handState.currentState} ;
    }
    
}

