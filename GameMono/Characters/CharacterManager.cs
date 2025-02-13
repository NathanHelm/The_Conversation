using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using System;
public class CharacterManager : StaticInstance<CharacterManager>
{
    private CharacterMono[] characterMono;
    private ClueMono[] clueMono;

    Dictionary<int, Dictionary<int, DialogueConversation>> characterIDtoConverationIdtoConversation = new Dictionary<int, Dictionary<int, DialogueConversation>>();

    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager m) => { m.characterManager = this; });
        base.OnEnable();
    }
    public override void m_Start()
    {
        SetUpCharacterIDtoConversationIdToConversation();
    }

    public void SetUpCharacterIDtoConversationIdToConversation() //method adds all occuring characterMono and their lines to the dictionary
    {
        characterMono = FindObjectsOfType<CharacterMono>();


        if(characterMono == null)
        {
            throw new NullReferenceException("no characters found");
        }

        for(int i = 0; i < characterMono.Length; i++)
        {
            int bodyId = characterMono[i].bodyID;
            AddDialogueObjToDict(characterMono[i].dialogueConversation, bodyId);
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
    }
    /*
     todo when loading and persistent information -im thinking ledger data- we should use this function
     */
    public void AddDialogueObjToDict(DialogueConversation[] dialogueConversation,int bodyId)
    {
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
    public DialogueConversation GetConversationOnCharacterID(int characterID, int characterConversationID)
    {
        return characterIDtoConverationIdtoConversation[characterID][characterConversationID];
    }
}

