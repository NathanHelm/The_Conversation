﻿using UnityEngine;
using System.Collections;
using Data;
public class StopCutsceneState : CutsceneState
{
    public override void OnEnter(CutsceneData data)
    {
        Debug.Log("stop cutscene");
    }
}

