using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;
public class QuestionResponseManager : StaticInstance<QuestionResponseManager>
{
	//based on relevant Character and Question output a conversation response.



	private DialogueData dialogueData;

	private DialogueScriptableObject player;
	
	private DialogueScriptableObject[] characters;
	private int currentID;




	//Dictionary<int, DialogueConversation> playerIDToDialogQuestions = new Dictionary<int, DialogueConversation>();
	Dictionary<int, Dictionary<int, DialogueConversation>> npcToQuestionDialogueNpc = new Dictionary<int, Dictionary<int, DialogueConversation>>();

    public override void Awake()
    {
		GameEventManager.INSTANCE.AddEventFunc<int, int, DialogueConversation>("getdialogueconversation", getDialogueConversation);
		base.Awake();
		SetUpQuestionResponseManager();
	}


    private void Start()
    {
       
    }


    public void SetUpQuestionResponseManager()
	{
		//all code is preprocessed
		//character id. 
		npcToQuestionDialogueNpc.Add(1, new Dictionary<int, DialogueConversation>());

		npcToQuestionDialogueNpc[1] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.

			new (new int[] { 1, 2, 3}, characters[0].character.dialogueConversations[0]),
			

		});

		



        //todo add npcs here


        //based on player questions, the character answer will be provided.


    }

	public Dictionary<int, DialogueConversation> AddQuestionsIDToCharacterAnswer((int[], DialogueConversation)[] keys) 
	{
		Dictionary<int, DialogueConversation> keyValuePairs = new Dictionary<int, DialogueConversation>();

		for(int i = 0; i < keys.Length; i++)
		{
			for (int j = 0; j < keys[i].Item1.Length; j++)
			{
				keyValuePairs.Add(keys[i].Item1[j], keys[i].Item2); //for each int in key array, add a responding key pair value.
			}
		}
		return keyValuePairs;
	}


    public DialogueConversation getDialogueConversation(int characterID, int playerQuestionID)
	{
		if(!npcToQuestionDialogueNpc.ContainsKey(characterID))
		{
			throw new KeyNotFoundException("character ID "+ characterID + " does not match with value");
		}
		if (!npcToQuestionDialogueNpc[characterID].ContainsKey(playerQuestionID))
		{
			throw new KeyNotFoundException("playerQuestion ID " + playerQuestionID + " does not match player question value for character " + characterID);
		}
		return npcToQuestionDialogueNpc[characterID][playerQuestionID];
		
	}


	
}

