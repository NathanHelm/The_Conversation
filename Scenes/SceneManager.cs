using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Data;
using System.Collections.Generic;
using System;
using ObserverAction;

public enum SceneNames
{
    SampleScene,
    MemoryScene,
    InterviewScene,
    VetHouseScene,
    HenryApartmentScene,

    
   
    None,
}
public class SceneManager : StaticInstance<SceneManager>, IExecution, IObserverData<ObserverAction.PlayerActions,CharacterMono>
{

    //This code will allow for easy transfer from one scene to another...
    public static SystemActionCall<SceneManager> onAfterSceneChange = new(); //this won't run after state manager. 
    public static SystemActionCall<SceneManager> onStartSceneManager = new(); //use this if you want to make any scene changes as it run AFTER statemanager

    public Dictionary<SceneNames, Type> enumSceneNameToSceneState = new Dictionary<SceneNames, Type>();
    



    public override void m_OnEnable()
    {
        PlayerData.INSTANCE?.playerRaycast.subjectCharacter.AddObserver(this);
        MManager.INSTANCE.onStartManagersAction.AddAction(mm => { mm.sceneManager = this; });

        enumSceneNameToSceneState.Add(SceneNames.SampleScene, typeof(SampleSceneState));
        enumSceneNameToSceneState.Add(SceneNames.VetHouseScene, typeof(VetHouseSceneState));
        enumSceneNameToSceneState.Add(SceneNames.InterviewScene, typeof(InterviewSceneState));
        enumSceneNameToSceneState.Add(SceneNames.MemoryScene, typeof(MemorySceneState));
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

    public void OnNotify(PlayerActions data, CharacterMono characterMono)
    {
        if (data == PlayerActions.onOmitRayCharacter)
        {
            MemoryData.CURRENTCHARACTERID = characterMono.bodyID;
        }
    }
}