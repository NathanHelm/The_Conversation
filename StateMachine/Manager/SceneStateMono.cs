

using System.Diagnostics;
using UnityEngine;
using System.Collections;
using Data;

public class SceneStateMono : StateMono<SceneData>
{
    private void OnEnable()
    {
        Value = SceneData.INSTANCE;
    }
    public void SwitchScene(State<SceneData> nextState, SceneNames nextSceneName)
    {
       if(SceneData.INSTANCE == null)
       {
         UnityEngine.Debug.LogError("Scene data is not found, cannot execute switch scene function.");
         return;
       }
       SceneData.INSTANCE.nextScene = nextSceneName;

       currentState.OnExit(Value);

      

       SceneManager.onAfterSceneChange.AddAction(sm => {

        currentState = nextState;

        currentState.Value = Value;

        currentState.OnEnter(Value);
        SceneData.INSTANCE.currentScene = nextSceneName;

        SceneManager.onAfterSceneChange.RemoveAllActions();
       });


        SceneManager.INSTANCE?.LoadScene(nextSceneName);

       // SceneManager.INSTANCE. Scene currentState.OnExit(Value);
    }

}