using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CutsceneTest : TestSetUp
{
    /*
    GameObject player;

   public void SetUp()
    {
        player = SetUpPlayer();

        //data stuff
        GameObject dataObj = new GameObject();
       
        dataObj.AddComponent<CutsceneData>();
        dataObj.AddComponent<DialogueData>();
        dataObj.AddComponent<Data.DimensionData>();
        dataObj.AddComponent<Data.PlayerData>();
        dataObj.AddComponent<Data.TriggerData>();
        //event stuff
        GameObject gameEventObj = new GameObject();

        var gameEventManager = gameEventObj.AddComponent<GameEventManager>();

        gameEventManager.Awake();

        GameObject stateObj = new GameObject();

        var stateMana = stateObj.AddComponent<StateManager>();

        stateMana.OnEnable();
     //   stateMana.Start();
        stateMana.m_Start();

        GameObject cutSceneObj = new GameObject();

        var cutsceneManager = cutSceneObj.AddComponent<CutsceneManager>();


        cutsceneManager.Awake();

        cutsceneManager.m_Start();



    }
    [UnityTest]
    public IEnumerator RaycastPlayerTestWithEnumeratorPasses()
    {

        SetUp();

        
    /*
        CutsceneManager.INSTANCE.SetCutSceneActionAndTime(new (System.Action, float)[] { new(() => { }, .5f) , new(() => { }, .5f) });

        CutsceneManager.INSTANCE.PlayCutscene();
        Assert.AreEqual(true, CutsceneManager.INSTANCE.stopStatesDict.Count > 0);
        Assert.AreEqual(true, CutsceneManager.INSTANCE.startStatesDict == null);

        Assert.AreEqual(typeof(PlayerIdleState), StateManager.INSTANCE.playerDataState.currentState.Value);
        Assert.AreEqual(typeof(PlayerIdleState), CutsceneManager.INSTANCE.stopStatesDict["PlayerState"]);
        //*
        CutsceneManager.INSTANCE.SetSnapShot(new (string, Type)[] {
           new ("PlayerState", typeof(PlayerMove2dState))
        });

        yield return new WaitForSeconds(1.1f);
        //changes to player 2d state instead of 3d state because we replace state in above*
        Assert.AreEqual(typeof(PlayerMove2dState), CutsceneManager.INSTANCE.startStatesDict["PlayerState"]); 
       
    }
     */
    

}
