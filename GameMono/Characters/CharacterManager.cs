﻿using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using System;
using Codice.Client.BaseCommands.BranchExplorer;
using System.IO;
using System.Linq;
using Persistence;
public class CharacterManager : StaticInstance<CharacterManager>, ISaveLoad
{
    private CharacterMono[] characterMono;
    private ClueMono[] clueMono;

    private Dictionary<int, Dictionary<int, DialogueConversation>> characterIDtoConverationIdtoConversation = new Dictionary<int, Dictionary<int, DialogueConversation>>();
    private Dictionary<int, Dictionary<int, MemoryStage>> characterStageIdToMemory = new Dictionary<int, Dictionary<int, MemoryStage>>();

    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager m) => { m.characterManager = this; });
        base.OnEnable();
    }
    public override void m_Start()
    {
        //FIRST: add dialogue object to start function
        
        SetUpCharacterIDtoConversationIdToConversation();
    }

    public void SetUpCharacterIDtoConversationIdToConversation() //method adds all occuring characterMono and their lines to the dictionary
    {
        
        characterMono = FindObjectsOfType<CharacterMono>();
        clueMono = FindObjectsOfType<ClueMono>();


        if(characterMono == null)
        {
            throw new NullReferenceException("no characters found");
        }

        for(int i = 0; i < characterMono.Length; i++)
        {
            int bodyId = characterMono[i].bodyID;
            AddDialogueObjToDict(characterMono[i].dialogueConversation, bodyId);
            AddMemoryFromCharacter(characterMono[i].memoryStages, bodyId);
            //add the body id, makes a new dictionary.
        }
        if(clueMono == null)
        {
            return;
        }
        for(int i = 0; i < clueMono.Length; i++) //we also add clue mono from game env
        {
            
            AddDialogueObjToDict(clueMono[i].vetClueConversation, clueMono[i].bodyID);
        }
        Debug.Log("LOG: Got all character from the scene, their id and conversation [see dialogue SO] are now in the character manager dictionary.");
        Debug.Log("LOG: obtained all memories from characters in scene. --data can be accessed from memory manager. ");
    }
    /*
     TODO when loading and persistent information -im thinking ledger data- we should use this function
     */
    public void AddDialogueObjToDict(DialogueConversation[] dialogueConversation,int bodyId)
    {
        if(characterIDtoConverationIdtoConversation.ContainsKey(bodyId))
        {
            Debug.LogError("duplicate character id's "+ bodyId + "in scene.");
            return;
        }
        characterIDtoConverationIdtoConversation.Add(bodyId, new Dictionary<int, DialogueConversation>());

        if (dialogueConversation == null) //character is removed from the dictionary if there is no conversation added to it.
        {
            characterIDtoConverationIdtoConversation.Remove(bodyId);
            throw new NullReferenceException("no dialogue conversation added to character bodyId" + bodyId);
        }
        for (int j = 0; j < dialogueConversation.Length; j++)
        {
            if (characterIDtoConverationIdtoConversation[bodyId].ContainsKey(dialogueConversation[j].ID))
            {
                throw new KeyNotFoundException("character bodyId " + bodyId + " already contains" + dialogueConversation[j].ID + "check conversation index " + j);
            }
            //adds the conversation to the respective body id.
            characterIDtoConverationIdtoConversation[bodyId].Add(dialogueConversation[j].ID, dialogueConversation[j]);
        }
    }
    public void AddMemoryFromCharacter(MemoryStage[] memoryStages,int bodyID)
    {
       
        characterStageIdToMemory.Add(bodyID,new Dictionary<int, MemoryStage>());
      
        for(int i = 0; i < memoryStages.Length; i++)
        {
        characterStageIdToMemory[bodyID].Add(memoryStages[i].memoryId,memoryStages[i]);
        }
        
        
    }
    public DialogueConversation GetConversationOnCharacterID(int characterID, int characterConversationID)
    {
        //making dummy dialogue conversation in case character is not in scene. 
        DialogueConversation dialogueConversationDummy = new DialogueConversation();
        DialogueObject dialogueObject = new DialogueObject();
        dialogueObject.line = "dummy dialogue because character doesn't exist in scene";
        dialogueConversationDummy.dialogueObjects = new DialogueObject[] { dialogueObject };


        if(!characterIDtoConverationIdtoConversation.ContainsKey(characterID))
        {
            Debug.LogError("character id " + characterID + " not found in scene, therefore we can't obtain it.");
            return dialogueConversationDummy;
        }
        if(!characterIDtoConverationIdtoConversation[characterID].ContainsKey(characterConversationID))
        {
            dialogueObject.line = "dummy dialogue because character: " + characterID + " does not conversation id " + characterConversationID;
            Debug.LogError("conversation id " + characterConversationID  +" therefore we can't obtain it.");
            return dialogueConversationDummy;
        }
        Debug.Log("LOG: obtained character ID: " + characterID + "and: " + characterConversationID); 
        return characterIDtoConverationIdtoConversation[characterID][characterConversationID];
    }

      public MemoryStage GetMemoriesOnCharacterID(int characterID, int memoryID)
    {
        //making dummy dialogue conversation in case character is not in scene. 
        MemoryStage dummymemorystage = new MemoryStage(); 
        dummymemorystage.stage = null;
        dummymemorystage.memoryId = 1000;

        if(!characterStageIdToMemory.ContainsKey(characterID))
        {
            Debug.LogError("character id " + memoryID + " not found in scene, therefore we can't obtain it. [for memory]");
            return dummymemorystage;
        }
        if(!characterIDtoConverationIdtoConversation[characterID].ContainsKey(memoryID))
        {
         Debug.LogError("character id " + characterID + " does not have memory id" + memoryID +" therefore we can't obtain memory. [for memory]");
            return dummymemorystage;
        }
        Debug.Log("LOG: obtained character ID: " + characterID + "and memory id: " + memoryID); 
        return characterStageIdToMemory[characterID][memoryID];
    }

    public (FileNames, JsonObject[])[] Save()
    {
        FileNames currentFile = FileNames.DialogueConversationFile;
        List<JsonDialogueConversationObject> jsonObjectsArr = new List<JsonDialogueConversationObject>();

        foreach(int key in characterIDtoConverationIdtoConversation.Keys)
        {
            Dictionary<int, DialogueConversation> idToDialogueConversation = characterIDtoConverationIdtoConversation[key];
            List<DialogueConversation> dialogueConversations = new List<DialogueConversation>();
            foreach(DialogueConversation dialogueVal in idToDialogueConversation.Values)
            {
               dialogueConversations.Add(dialogueVal);
            }
            
            JsonDialogueConversationObject jsonDialogueConversationObject = 
            new JsonDialogueConversationObject(dialogueConversations, key);

            jsonObjectsArr.Add(jsonDialogueConversationObject);
            
        }


        return new (FileNames, JsonObject[])[] { 
            
            new (currentFile, jsonObjectsArr.ToArray()) //DialogueConversationFile is handling characterid --> dialogueId --> dialogueObj...
            
        };
       /*
       here I am saving 
       hopefully in json, I will achieve this:

       {

       "characterID_1":{
        "dialogueID_1" : [
        
       ...DialogueConversation...

        
        
        ]
       
       }
       ...

       "characterID_2":{
       
       }
       

       }
       
       
       
       

       */
        throw new NotImplementedException();
    }

    public void Load() //get contents from file handler
    {
        //1) we have obtained the json from the desired file
        List<JsonDialogueConversationObject> jsonDialogueConversationObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonDialogueConversationObject>(FileNames.DialogueConversationFile);
        //2) iterate through list and add to hashmap 
        for(int i = 0; i < jsonDialogueConversationObjects.Count; i++)
        {
           AddDialogueObjToDict(jsonDialogueConversationObjects[i].dialogueObjects.ToArray(), jsonDialogueConversationObjects[i].Id);
        }
    }

   
}

