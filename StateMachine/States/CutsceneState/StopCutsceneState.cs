using UnityEngine;
using System.Collections;
using Data;
public class StopCutsceneState : CutsceneState
{
    public override void OnEnter(CutsceneData data)
    {
      CutsceneManager.INSTANCE.EndCutsceneOnState();
    }
}

