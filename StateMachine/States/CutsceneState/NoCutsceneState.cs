using UnityEngine;
using System.Collections;
using Data;

public class NoCutsceneState : CutsceneState
{
    public override void OnEnter(CutsceneData data)
    {
        Debug.Log("cutscene in idle mode");
        base.OnEnter(data);
    }
}

