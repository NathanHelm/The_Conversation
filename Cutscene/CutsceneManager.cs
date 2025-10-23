using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Data;
public class CutsceneManager : StaticInstance<CutsceneManager>, IExecution
{
    private object[] previousStates = new object[] { };

    private (string stateMonoName, Type state)[] oPauseState = new (string stateMonoName, Type state)[] { };
    private (string stateMonoName, Type state)[] oPreviousState = new (string stateMonoName, Type state)[] { };

    private List<string> removeStateOnRunPreviousState = new List<string>();
    private List<string> removeStateOnStopState = new List<string>();

    private Dictionary<string, Type> stateMachineStateName = new Dictionary<string, Type>();


    public override void m_Start()
    {
    }
    //below we set what states on play and stop cutscene will be
    public void SetOPreviousState((string stateMonoName, Type state)[] oPreviousState)
    {
        this.oPreviousState = oPreviousState;
    }
    //running states on pause
    public void PauseAllStates()
    {
        //get previous states before transitioning current states to idle
        previousStates = StateManager.INSTANCE.SnapShotCurrentStates();

        var idlestates = StateManager.INSTANCE.stopStates;

        //here we are getting all of the idle states
        stateMachineStateName = StateManager.INSTANCE.GetStateHashmap(idlestates);

        //here we are getting our pause states and overriding them with the idle states.
        foreach ((string, Type) overridePauseState in oPauseState)
        {
            stateMachineStateName[overridePauseState.Item1] = overridePauseState.Item2;
        }
        //here we are removing the states we don't want to change in the cutscene.
        RemoveStateMono(removeStateOnStopState);


        RunState(); //running the states (using gamemanager.onevent)

        removeStateOnStopState = new List<string>(); //reset states 
        oPauseState = new (string stateMonoName, Type state)[] { }; //reset override states

    }

    //running previous states when cutscene ends
    public void PlayAllPreviousStates()
    {
        Debug.Log("playing previous states!");
        if (previousStates.Length == 0)
        {
            Debug.LogError("previous states not found");
        }
        if (previousStates.Any(x => x is PlayCutsceneState))
        {
            Debug.LogWarning("removed play cutscene state from captured previous states!");
            RemoveCapturedCutsceneState();
        }

        stateMachineStateName = StateManager.INSTANCE.GetStateHashmap(previousStates);

        foreach ((string, Type) overridePreviousState in oPreviousState)
        {
            stateMachineStateName[overridePreviousState.Item1] = overridePreviousState.Item2;
        }
        RemoveStateMono(removeStateOnRunPreviousState);
        RunState();


        oPreviousState = new (string stateMonoName, Type state)[] { };
        removeStateOnRunPreviousState = new List<string>(); //reset string
    }

    public void RemoveCapturedStateMono(string s) //states that will not run when returning to previous state when cutscene is over
    {
        //for example to ledger that is running stop cutscene does not need to be overriden with the "returning" ledger state. 
        removeStateOnRunPreviousState.Add(s);
    }
    public void RemoveStopStateMono(string s) //states that will not run when entering a cutscene.
    {
        //if a cutscene is running, there might be states which will be enabled.
        removeStateOnStopState.Add(s);
    }
    private void RemoveStateMono(List<string> s)
    {
        for (int i = 0; i < s.Count; i++)
        {
            stateMachineStateName.Remove(s[i]);
        }

    }


    public void ResetStateMachineState()
    {
        oPreviousState = new (string stateMonoName, Type state)[] { };
        oPauseState = new (string stateMonoName, Type state)[] { };
        removeStateOnStopState = new();
        removeStateOnRunPreviousState = new();
    }


    private void RunState() //runs the states in state dictionary
    {

        var values = stateMachineStateName.Values;
        foreach (Type state in values)
        {
            GameEventManager.INSTANCE.OnEvent(state);
        }
    }

    public void RemoveCapturedCutsceneState()
    {
        RemoveCapturedStateMono("CutsceneState");
    }






}
