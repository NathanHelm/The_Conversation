using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using System;
public class CharacterManager : StaticInstance<CharacterManager>
{
    private CharacterMono[] characterMono;

    Dictionary<int, Dictionary<int, DialogueConversation>> characterIDtoConverationIdtoConversation = new Dictionary<int, Dictionary<int, DialogueConversation>>();

    public override void Awake()
    {
        base.Awake();
        
    }

    public override void m_Start()
    {
        GameEventManager.INSTANCE.AddEventFunc<int, int, DialogueConversation>("getconversationoncharacterid", GetConversationOnCharacterID);
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
            //add the body id, makes a new dictionary.
            characterIDtoConverationIdtoConversation.Add(bodyId, new Dictionary<int, DialogueConversation>());
            
            DialogueConversation[] dialogueConversation = characterMono[i].dialogueConversation;

            if(dialogueConversation == null) //character is removed from the dictionary if there is no conversation added to it.
            {
                characterIDtoConverationIdtoConversation.Remove(bodyId);
                throw new NullReferenceException("no dialogue conversation added to character " + characterMono[i].name);
            }
            for(int j = 0; j < dialogueConversation.Length; j++)
            {
                if (characterIDtoConverationIdtoConversation[bodyId].ContainsKey(dialogueConversation[j].ID))
                {
                    throw new KeyNotFoundException("character " + characterMono[i].name + " already contains" + dialogueConversation[j].ID +"check conversation index " + j);
                }
                //adds the conversation to the respective body id.
                characterIDtoConverationIdtoConversation[bodyId].Add(dialogueConversation[j].ID, dialogueConversation[j]);
            }
        }
    }
    public DialogueConversation GetConversationOnCharacterID(int characterID, int characterConversationID)
    {
        return characterIDtoConverationIdtoConversation[characterID][characterConversationID];
    }
}

