using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Data;
public class CutsceneManager : StaticInstance<CutsceneManager>
{


    public Dictionary<string, Type> stopStatesDict { get; private set; }
    public Dictionary<string, Type> startStatesDict { get; private set; }

    private IEnumerator cutsceneEnumerator;


    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager m) => { m.cutsceneManager = this; });

        base.OnEnable();
    }


    private IEnumerator ActionAndTime((Action, float)[] actionAndTime)
    {
        StopStates(); //all states are paused as cutscene in playing
        for(int i = 0; i < actionAndTime.Length; i++)
        {
            actionAndTime[i].Item1();
            yield return new WaitForSeconds(actionAndTime[i].Item2);
            
        }
        RunSnapShotStates(); //revert back to previous states.
        GameEventManager.INSTANCE.OnEvent(typeof(StopCutsceneState));
    }

    private IEnumerator ActionAndCondition((Action, bool)[] actionAndTime)
    {
        StopStates(); //all states are paused as cutscene in playing
        int i = 0;
        while (i < actionAndTime.Length)
        {
            actionAndTime[i].Item1();
            if (actionAndTime[i].Item2 == true)
            {
                i++;
            }
            yield return new WaitForFixedUpdate();
        }
        RunSnapShotStates(); //revert back to previous states.
        GameEventManager.INSTANCE.OnEvent(typeof(StopCutsceneState));
    }

    public void PlayCutscene()
    {
       StartCoroutine(cutsceneEnumerator = ActionAndTime(CutsceneData.INSTANCE.cutsceneActionsAndTimeTurnaround));
    }
    public void PauseCutscene()
    {
        //cutscene with continue booling is true.
        StartCoroutine(cutsceneEnumerator = ActionAndCondition(CutsceneData.INSTANCE.cutsceneActionAndConditions));
    }

    public void SetCutSceneActionAndTime((Action, float)[] cutSceneActionAndTime)
    {
        CutsceneData.INSTANCE.cutsceneActionsAndTimeTurnaround = cutSceneActionAndTime;
    }
    public void SetCutSceneConditions((Action, bool)[] cutSceneActionAndCondition)
    {
        CutsceneData.INSTANCE.cutsceneActionAndConditions = cutSceneActionAndCondition;
    }

    public void SetSnapShot((string, Type)[] snapShotThatAreReplaced)
    {
        CutsceneData.INSTANCE.replaceSnapShots = snapShotThatAreReplaced;
    }

    public void StopCutscene()
    {
        //in the event you must stop the cutscene.
        StopCoroutine(cutsceneEnumerator);
    }
    private void StopStates()
    {
        CutsceneData.INSTANCE.snapShotCutscene = StateManager.INSTANCE.SnapShotCurrentStates(); //make snapshot.
        var stopStates = StateManager.INSTANCE.stopStates;
        stopStatesDict = StateManager.INSTANCE.GetStateHashmap(stopStates);
        Type[] values = stopStatesDict.Values.ToArray();

        for(int i = 0; i < values.Length; i++)
        {
           GameEventManager.INSTANCE.OnEvent(values[i]);
        }
    }
    private void RunSnapShotStates()
    {
        //cutscene states return back to values BEFORE cutscene occured
        var snap = CutsceneData.INSTANCE.snapShotCutscene.ToList();

        for(int i = 0; i < snap.Count; i++)
        {
           if(snap[i] is CutsceneState)
           {
                Debug.LogError("removing cutscene state");
                snap.RemoveAt(i);
           }
        }


        var replaceSnap = CutsceneData.INSTANCE.replaceSnapShots;
        
        if (snap == null || snap?.Count == 0)
        {
            throw new Exception("snapshot data is either null or length 0");
        }
        startStatesDict = StateManager.INSTANCE.GetStateHashmap(snap.ToArray());
        

        if (replaceSnap != null)
        {
            if (replaceSnap.Length > 0)
            {
                for (int i = 0; i < replaceSnap.Length; i++)
                {
                    if (startStatesDict.ContainsKey(replaceSnap[i].Item1))
                    {
                        startStatesDict[replaceSnap[i].Item1] = replaceSnap[i].Item2;
                    }
                    else
                    {
                        Debug.LogError("Replacment " + replaceSnap[i].Item1 + " didn't work");
                    }
                }
            }
        }
        Type[] values = startStatesDict.Values.ToArray();

        for (int i = 0; i < values.Length; i++)
        {
            GameEventManager.INSTANCE.OnEvent(values[i]);
            //give up

        }
    }

}
