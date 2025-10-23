using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerRaycastTest : TestSetUp
{
    // A Test behaves as an ordinary method
    private GameObject player, clue;
    private PlayerRaycast playerRaycast;
    [Test]
    public void RaycastPlayerTestSimplePasses()
    {
        // Use the Assert class to test conditions
     
    }
    private void SetUp()
    {
        player = SetUpPlayer();
        clue = SetUpClue();
        //THIS IS TEMPORARY!, get component is added to set up when things WORK.
        playerRaycast = player.AddComponent<PlayerRaycast>();

        GameObject dialogueDataObj = new GameObject(); //making triggerdata object

        var dialogueData = dialogueDataObj.AddComponent<DialogueData>();

        dialogueData.Start();


        GameObject ledgerDataObj = new GameObject();

        var ledgerData = ledgerDataObj.AddComponent<LedgerData>();

        ledgerData.m_OnEnable();

        ledgerData.m_Start();


        GameObject ledgerManaObj = new GameObject(); //making triggerdata object

        var ledgerManager = ledgerManaObj.AddComponent<LedgerManager>();

        ledgerManager.m_Start();

     


    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator RaycastPlayerTestWithEnumeratorPasses()
    {
        SetUp();

        Vector3 mousePos = new Vector3(0, 1, 0);
        clue.transform.position = new Vector3(0, 30, 0);
        player.transform.position = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.2f);

        playerRaycast.OmitRaycast(mousePos);
        
        Assert.AreEqual(LayerMask.NameToLayer("cluecollider"), clue.layer);
        Assert.AreEqual(DialogueData.INSTANCE.currentQuestionID, 0);
        Assert.AreEqual(DialogueData.INSTANCE.currentCharacterID, 3);
        Assert.AreEqual(LedgerData.INSTANCE.ledgerImages.Count, 1);
        



        yield return null;
    }
    [UnityTest]
    public IEnumerator RayCastPlayerTwoCluesHitAtSameTime()
    {
        if (player == null)
        {
            Debug.Log("setting up for new test");
            SetUp();

        }

        Vector3 mousePos = new Vector3(0, 1, 0);
        clue.transform.position = new Vector3(0, 30, 0);
        player.transform.position = new Vector3(0, 0, 0);

        GameObject anotherClue = SetUpClue();

        anotherClue.transform.position = new Vector3(0, 20, 0); //two clue mono stacked on top of each other
        var clueMono = anotherClue.GetComponent<ClueMono>();
        clueMono.bodyID = 14;
        yield return new WaitForSeconds(0.2f);
     
        playerRaycast.OmitRaycast(mousePos);
      
        Assert.AreEqual(14, playerRaycast.hitClues[0].bodyID);
        Assert.AreEqual(LedgerData.INSTANCE.ledgerImages.Count, 1);





        yield return null;
    }
}
