using UnityEngine;
using System.Collections;
using Data;
using System;

public class PlayCutsceneState : CutsceneState
{
    public override void OnEnter(CutsceneData data)
    {
        Debug.Log("play cutscene");
        CutsceneManager.INSTANCE.PauseAllStates();
    }
}

