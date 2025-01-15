using UnityEngine;
using System.Collections;
using Data;
public class StopCutsceneState : State<CutsceneState>
{
    public override void OnEnter(CutsceneState data)
    {
        Debug.Log("stop cutscene");
    }
}

