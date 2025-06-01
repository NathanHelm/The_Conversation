

using System.Diagnostics;
using UnityEngine;
using System.Collections;
using Data;
using UnityEngine.InputSystem.XR.Haptics;


public class SceneStateMono : StateMono<SceneData>
{
  public static State<SceneData> currentSceneState;
  private void OnEnable()
  {
    Value = SceneData.INSTANCE;
   
  }
    


    public void SwitchScene(State<SceneData> nextState, SceneNames nextSceneName)
  {
    if (SceneData.INSTANCE == null)
    {
      UnityEngine.Debug.LogError("Scene data is not found, cannot execute switch scene function.");
      return;
    }
    SceneData.INSTANCE.nextScene = nextSceneName;

     currentSceneState.OnExit(Value);



    SceneManager.onAfterSceneChange.AddAction(sm => //after scene runs
    {
      SceneManager.onStartSceneManager.AddAction(sm => //function runs in MManager
      {

        currentSceneState = nextState;

        currentSceneState.Value = Value;

        currentSceneState.OnEnter(Value);
        
        SceneData.INSTANCE.currentScene = nextSceneName;

        SceneManager.onAfterSceneChange.RemoveAllActions();
        SceneManager.onStartSceneManager.RemoveAllActions();

      });

    });


    SceneManager.INSTANCE?.LoadScene(nextSceneName);

    // SceneManager.INSTANCE. Scene currentState.OnExit(Value);
  }

}