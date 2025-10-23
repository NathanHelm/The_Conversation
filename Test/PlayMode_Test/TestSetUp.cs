using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestSetUp
{
    private string[] defaultExampleDialog = new string[] { "line is 1", "line is 2", "line is 3" };

    public GameObject SetUpCharacter()
    {
        GameObject characterTest = new GameObject(); //making player object
        characterTest.transform.localScale = new Vector3(1, 1, 1);
        characterTest.transform.position = new Vector3(1, 1, 1);
        characterTest.AddComponent<BoxCollider>();
       var rigid = characterTest.AddComponent<Rigidbody>();
        rigid.useGravity = false;



        try
        {
            characterTest.AddComponent<CharacterMono>().SetCharacterScriptableObject(SetUpCharacterDialogueScriptableObject(defaultExampleDialog));
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e + "handling it...");
            var characterMono = characterTest.GetComponent<CharacterMono>();
            characterMono.SetCharacterScriptableObject(SetUpCharacterDialogueScriptableObject(defaultExampleDialog));
            //characterMono.OnEnable();
            characterMono.bodyID = 1; //1 signifies its a player.

        }

        return characterTest;
    }
   
    public GameObject SetUpPlayer()
    {
        GameObject characterTest = new GameObject(); //making player object
        characterTest.transform.position = new Vector3(1, 1, 1);
        characterTest.transform.localScale = new Vector3(1, 1, 1);

        characterTest.AddComponent<BoxCollider>();
        var rigid = characterTest.AddComponent<Rigidbody>();
        rigid.useGravity = false;
       // characterTest.AddComponent<PlayerRaycast>();
        characterTest.tag = "Player";
        
        
        return characterTest;
    }
    public CharacterScriptableObject SetUpCharacterDialogueScriptableObject(string[] exampleDialog)
    {
        CharacterScriptableObject characterScriptableObject = ScriptableObject.CreateInstance<CharacterScriptableObject>();
        if (exampleDialog == null)
        {
            throw new NullReferenceException("example dialogue is null");
        }
        if (exampleDialog.Length == 0)
        {
            throw new Exception("exampleDialogue is size 0? ");
        }

   
        Character character = new Character();

        DialogueConversation dialogueConversation1 = new DialogueConversation();

        DialogueObject[] dialogueObjectArr = new DialogueObject[exampleDialog.Length];

        for(int i = 0; i < exampleDialog.Length; i++) //fill example dialog with dialogue conversation dialogue
        {
            DialogueObject temp = new()
            {
                line = exampleDialog[i]
            };
            dialogueObjectArr[i] = temp;
        }

        dialogueConversation1.dialogueObjects = dialogueObjectArr;

        characterScriptableObject.character = character;

        characterScriptableObject.character.dialogueConversations = new DialogueConversation[] { dialogueConversation1 };

        return characterScriptableObject;
    }
    public ClueScriptableObject SetUpClueScriptableObject(string[] exampleDialog)
    {
        ClueScriptableObject clueScriptableObject = ScriptableObject.CreateInstance<ClueScriptableObject>();
        if (exampleDialog == null)
        {
            throw new NullReferenceException("example dialogue is null");
        }
        if (exampleDialog.Length == 0)
        {
            throw new Exception("exampleDialogue is size 0? ");
        }

        DialogueConversation dialogueConversation1 = new DialogueConversation();

        DialogueObject[] dialogueObjectArr = new DialogueObject[exampleDialog.Length];

        for (int i = 0; i < exampleDialog.Length; i++) //fill example dialog with dialogue conversation dialogue
        {
            DialogueObject temp = new()
            {
                line = exampleDialog[i]
            };
            dialogueObjectArr[i] = temp;
        }

        dialogueConversation1.dialogueObjects = dialogueObjectArr;

        clueScriptableObject.dialogConversation =  dialogueConversation1;

        return clueScriptableObject;
    }
    public DialogueScriptableObject SetUpDialogueScriptableObject(string[] exampleDialog)
    {
        DialogueScriptableObject dialogueScriptableObject = ScriptableObject.CreateInstance<DialogueScriptableObject>();
        if (exampleDialog == null)
        {
            throw new NullReferenceException("example dialogue is null");
        }
        if (exampleDialog.Length == 0)
        {
            throw new Exception("exampleDialogue is size 0? ");
        }
        DialogueConversation dialogueConversation = new();
        DialogueObject[] dialogueObjectArr = new DialogueObject[exampleDialog.Length];

        for (int i = 0; i < exampleDialog.Length; i++) //fill example dialog with dialogue conversation dialogue
        {
            DialogueObject temp = new()
            {
                line = exampleDialog[i]
            };
            dialogueObjectArr[i] = temp;
        }
        dialogueScriptableObject.dialogueConversations = dialogueConversation;
        return dialogueScriptableObject;

    }
    public GameObject SetUpTrigger()
    {
        GameObject triggerTest = new GameObject(); //making trigger object
        triggerTest.transform.position = new Vector3(1, 1, 1);
        triggerTest.transform.localScale = new Vector3(10, 10, 10);
        var trigr = triggerTest.AddComponent<Trigger>();
        var boxCol = triggerTest.AddComponent<BoxCollider>();
        boxCol.isTrigger = true;
        trigr.bodiesOnTrigger = new List<BodyMono>();
        trigr.charactersOnTrigger = new List<CharacterMono>();
        return triggerTest;
    }

    public GameObject SetUpClue()
    {
        GameObject clueTest = new GameObject(); //making player object
        clueTest.transform.position = new Vector3(1, 1, 1);
        clueTest.transform.localScale = new Vector3(10, 10, 10);
        clueTest.AddComponent<BoxCollider>();
        clueTest.AddComponent<Rigidbody>();
        ClueMono clueMonoTest = null;

        try
        {
            clueTest.AddComponent<ClueMono>().SetClueScriptableObject(SetUpClueScriptableObject(defaultExampleDialog));
        }
        catch (Exception e)
        {
            Debug.Log(e + "handling it...");
            clueMonoTest = clueTest.GetComponent<ClueMono>();
            clueMonoTest.SetClueScriptableObject(SetUpClueScriptableObject(defaultExampleDialog));
          

        }
        finally
        {
            clueMonoTest = clueTest.GetComponent<ClueMono>();
            clueMonoTest.bodyID = 3; //1 signifies its a clue.
            clueTest.layer = LayerMask.NameToLayer("cluecollider");
            clueMonoTest.OnEnable();
        }
       

        return clueTest;
    }


}
