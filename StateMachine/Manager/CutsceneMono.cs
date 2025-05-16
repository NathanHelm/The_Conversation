using UnityEngine;
using System.Collections;
using Data;
public class CutsceneMono :  StateMono<CutsceneData>
{
    public void OnEnable()
    {
        Value = CutsceneData.INSTANCE;
    }
    
    public void SwitchScene(CutsceneState nextState)
    {
       // currentState.OnEnter(Value);
       // SceneManager.INSTANCE. Scene currentState.OnExit(Value);
    }
}

