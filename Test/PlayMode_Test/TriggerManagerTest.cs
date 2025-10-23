using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Data;
using System;

public class TriggerManagerTest : TestSetUp
{
    //setup player
    private GameObject playerTest;
    private GameObject characterTest;
    private GameObject triggerTest;
   
    private void Setup()
    {
        Debug.Log("set up is setting up");
     
        characterTest = SetUpCharacter();
        
        

        playerTest = SetUpPlayer();

        


        triggerTest = SetUpTrigger();

        GameObject triggerActionManagerObj = new GameObject(); //make trigger action manager
        var triggerActionManager = triggerActionManagerObj.AddComponent<TriggerActionManager>(); //to run conversation state, id is 22.
        triggerActionManager.m_Start();


        GameObject triggerManagerObj = new(); //set up trigger manager
        var triggerManager = triggerManagerObj.AddComponent<TriggerManager>();

        triggerManager.m_Awake();


        GameObject dialogueDataObj = new GameObject(); //making triggerdata object

        var dialogueData = dialogueDataObj.AddComponent<DialogueData>();

        dialogueData.Start();

        GameObject triggerDataObj = new GameObject(); //make trigger data object

        var triggerData = triggerDataObj.AddComponent<TriggerData>();

        triggerData.m_Awake();

        GameObject stateManagerObj = new GameObject(); //make state manager (set up trigger action)

        stateManagerObj.AddComponent<StateManager>();


        GameObject uiDataObj = new GameObject(); //make ui data object

        var uiData = triggerDataObj.AddComponent<UIData>();


        GameObject uiManager = new GameObject(); //make uiManager

        uiManager.AddComponent<UIManager>();

        GameObject dialogueManagerObj = new GameObject(); //make dialogue manager (to set up no conversation state!)


        dialogueManagerObj.AddComponent<DialogueManager>();

        
        TriggerData.INSTANCE.m_Awake();
        TriggerData.INSTANCE.m_OnEnable();
        UIData.INSTANCE.m_OnEnable();



        TriggerManager.INSTANCE.m_Start();

        UIManager.INSTANCE.m_OnEnable();

        UIManager.INSTANCE.m_Start();

        DialogueManager.INSTANCE.m_Start();

        StateManager.INSTANCE.m_Awake();


       







    }

    // A Test behaves as an ordinary method
    [Test]
    public void TriggerManagerTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TriggerManagerTestWithEnumeratorPasses()
    {
        /*
         testing when player is on trigger
         */
        Setup();

        triggerTest.transform.position = new Vector3(0,100, 0);
        characterTest.transform.position = new Vector3(0,200, 0);  
        playerTest.transform.position = new Vector3(0, 300, 0);

        triggerTest.transform.position = new Vector3(0, 0, 0);
        characterTest.transform.position = new Vector3(0, 2, 0); //player hits trigger.
        playerTest.transform.position = new Vector3(0, 0, 0); 

        string triggerThatsIsInTriggerName = triggerTest.name = "my new name";

        yield return new WaitForSeconds(0.5f);
        var tData = TriggerData.INSTANCE;

        Trigger trigger = triggerTest.GetComponent<Trigger>();

        trigger.Start();

        Assert.AreEqual(tData.triggerOnTrigger.name, triggerThatsIsInTriggerName); //the trigger that's currently 'on trigger' IS EQUAL the same one with variable TriggerOnTrigger in data file.

        Assert.AreEqual(tData.triggerOnTrigger.gameObject.GetHashCode(), triggerTest.GetHashCode()); //check hashcode

        Assert.AreEqual(1,trigger.charactersOnTrigger.Count);

        Assert.AreEqual(1,trigger.bodiesOnTrigger.Count);


        
        yield return null;
    }
    [UnityTest]
    public IEnumerator TestTriggerManagerTwoOrMoreCharacterInTrigger()
    {
        Setup();
        

        Trigger trigger = triggerTest.GetComponent<Trigger>();
      
            triggerTest.transform.position = new Vector3(0, 100, 0);

            GameObject[] charactersForTest = new GameObject[] { SetUpCharacter(), SetUpCharacter(), SetUpCharacter() }; //setting up three or more characters. 

            for (int i = 0; i < 3; i++)
            {
                charactersForTest[i].transform.position = new Vector3(0, i, 0);
            }
            triggerTest.transform.position = new Vector3(0, 0, 0);
            trigger.Start();
            for (int i = 0; i < 3; i++)
            {
                charactersForTest[i].transform.position = Vector3.zero; //characters are hitting the trigger.

            }
        

        yield return new WaitForSeconds(0.1f);

       

        Assert.AreEqual(1, trigger.bodiesOnTrigger.Count);
        Assert.AreEqual(1, trigger.charactersOnTrigger.Count);

       




        yield return null;
    }
}
