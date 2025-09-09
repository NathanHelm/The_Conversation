

using System.Diagnostics;
using UnityEngine;
using System.Collections;
using Data;
using UnityEngine.InputSystem.XR.Haptics;


public class SceneStateMono : StateMono<SceneData>
{
 
 
    


  public void SwitchScene(State<SceneData> nextState, SceneNames nextSceneName)
  {
    if (SceneData.INSTANCE == null)
    {
      UnityEngine.Debug.LogError("Scene data is not found, cannot execute switch scene function.");
      return;
    }

    SceneData.INSTANCE.nextScene = nextSceneName;

    SceneData.CURRENTSCENESTATE.OnExit(SceneData.INSTANCE);



    SceneManager.onAfterSceneChange.AddAction(sm => //after scene runs
    {
      SceneManager.onStartSceneManager.AddAction(sm => //function runs in MManager
      {
        SceneData.CURRENTSCENESTATE = nextState;
        SceneData.CURRENTSCENESTATE.OnEnter(SceneData.INSTANCE);
        ActionController.AFTERENTERINGSCENE(SceneManager.INSTANCE);
        SceneData.CURRENTSCENE = nextSceneName;
        SceneManager.onAfterSceneChange.RemoveAllActions();
        SceneManager.onStartSceneManager.RemoveAllActions();

      });

    });


    SceneManager.INSTANCE?.LoadScene(nextSceneName);

    // SceneManager.INSTANCE. Scene currentState.OnExit(Value);
  }

}