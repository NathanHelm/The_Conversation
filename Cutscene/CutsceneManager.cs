using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Data;
public class CutsceneManager : StaticInstance<CutsceneManager>
{
    private object[] idlestates = new object[] {};
    private object[] previousStates = new object[] {};

    private (string stateMonoName, Type state)[] oPauseState = new (string stateMonoName, Type state)[] {}; 
    private (string stateMonoName, Type state)[] oPreviousState = new (string stateMonoName, Type state)[] {}; 

    private List<string> removeStateOnRunPreviousState = new List<string>(); 
    private List<string> removeStateOnStopState = new List<string>(); 

    private Dictionary<string, Type> stateMachineStateName = new Dictionary<string, Type>();

    
    public override void m_Start()
    {
       idlestates = StateManager.INSTANCE.stopStates;
       stateMachineStateName = StateManager.INSTANCE.GetStateHashmap(idlestates);
    }
    //below we set what states on play and stop cutscene will be
    public void SetOPauseState((string stateMonoName, Type state)[] oPauseState) //set both fields before running code 
    {
        this.oPauseState = oPauseState;
    }
    public void SetOPreviousState((string stateMonoName, Type state)[] oPreviousState)
    {
        this.oPreviousState = oPreviousState;
    }
    //running states on pause
    public void PauseAllStates()
    {
        //get previous states before transitioning current states to idle
        previousStates = StateManager.INSTANCE.SnapShotCurrentStates();

        idlestates = StateManager.INSTANCE.stopStates;
        stateMachineStateName = StateManager.INSTANCE.GetStateHashmap(idlestates);
       

        foreach((string,Type) overridePauseState in oPauseState)
        {
           stateMachineStateName[overridePauseState.Item1] = overridePauseState.Item2;
        }

        RemoveStateMono(removeStateOnStopState);


        removeStateOnStopState = new List<string>(); //reset states 

        RunState();

        oPauseState = new (string stateMonoName, Type state)[] {};

    }
   
    //running previous states when cutscene ends
    public void PlayAllPreviousStates()
    {
        if(previousStates.Length == 0)
        {
            Debug.LogError("previous states not found");
        }
        stateMachineStateName = StateManager.INSTANCE.GetStateHashmap(previousStates);

        foreach((string,Type) overridePreviousState in oPreviousState)
        {
           stateMachineStateName[overridePreviousState.Item1] = overridePreviousState.Item2;
        }

        RemoveStateMono(removeStateOnRunPreviousState);
        removeStateOnRunPreviousState = new List<string>(); //reset string

        RunState();
        oPreviousState = new (string stateMonoName, Type state)[] {};
    }

    public void RemovePreviousStateMono(string s) //states that will not run when returning to previous state when cutscene is over
    {
        //for example to ledger that is running stop cutscene does not need to be overriden with the "returning" ledger state. 
       removeStateOnStopState.Add(s);
    }
    public void RemoveStopStateMono(string s) //states that will not run when entering a cutscene.
    {
        //if a cutscene is running, there might be states which will be enabled.
        removeStateOnRunPreviousState.Add(s);
    }
    private void RemoveStateMono(List<string> s)
    {
        for(int i = 0; i < s.Count; i++)
        {
        stateMachineStateName.Remove(s[i]);
        }
       
    }

    public void ResetStateMachineState()
    {
        oPreviousState = new (string stateMonoName, Type state)[] {};
        oPauseState = new (string stateMonoName, Type state)[] {};
    }

   
    private void RunState() //runs the states in state dictionary
    {
       var values = stateMachineStateName.Values;
        foreach(Type state in values)
        {
            GameEventManager.INSTANCE.OnEvent(state);
        }
    }

    public void LedgerDialog()
    {
       SetOPreviousState(new (string, Type)[] { new("DimensionState", typeof(TransitionTo3d)), new("PlayerState", typeof(PlayerLook3dState)), new ("DialogueState",typeof(NoConversationState))});
       RemovePreviousStateMono("CutsceneState");
       RemoveStopStateMono("CutsceneState");
       RemoveStopStateMono("LedgerState");
       RemovePreviousStateMono("LedgerState");
       RemoveStopStateMono("HandState");
       RemovePreviousStateMono("HandState");
    }






}
