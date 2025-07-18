using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Data;
using System.Collections.Generic;
using System;

public enum SceneNames{

    VetHouseScene,
    InterviewScene,
    HenryApartmentScene,
}
public class SceneManager : StaticInstance<SceneManager>, IExecution
{

    //This code will allow for easy transfer from one scene to another...
    public static SystemActionCall<SceneManager> onAfterSceneChange = new(); //this won't run after state manager. 
    public static SystemActionCall<SceneManager> onStartSceneManager = new(); //use this if you want to make any scene changes as it run AFTER statemanager

    public Dictionary<SceneNames, Type> enumSceneNameToSceneState = new Dictionary<SceneNames, Type>();



    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction(mm => { mm.sceneManager = this; });
        enumSceneNameToSceneState.Add(SceneNames.VetHouseScene, typeof(VetHouseSceneState));
        enumSceneNameToSceneState.Add(SceneNames.InterviewScene, typeof(InterviewSceneState));
        base.m_OnEnable();
    }
    public override void m_Start()
    {
        
        onStartSceneManager.RunAction(this);
        base.m_Start();
    }

    public void LoadScene(SceneNames sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += Test;
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneName);
    }
    private void Test(Scene scene, LoadSceneMode loadScene)
    {
        onAfterSceneChange.RunAction(this);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= Test;
    }

   

}