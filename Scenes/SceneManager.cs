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
public class SceneManager : StaticInstance<SceneManager>{

    //This code will allow for easy transfer from one scene to another...
    public static SystemActionCall<SceneManager> onAfterSceneChange = new();
    
    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void m_Start()
    {
        base.m_Start();
    }
    public void LoadScene(SceneNames sceneName)
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadScene)=> {onAfterSceneChange.RunAction(this);};
      UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneName);
    }

   

}