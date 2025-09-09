using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;
public class QuestionResponseManager : StaticInstance<QuestionResponseManager>, ISaveLoad, IExecution
{
	//based on relevant Character and Question id; output a conversation response.

	private DialogueData dialogueData;

	Dictionary<int, Dictionary<int, DialogueConversation>> npcToQuestionDialogueNpc = new Dictionary<int, Dictionary<int, DialogueConversation>>();

	public override void m_OnEnable()
	{
		MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) =>
		{
			m.questionResponseManager = this;
		});

		base.m_OnEnable();

	}

	public override void m_Start()
	{
		dialogueData = DialogueData.INSTANCE;
		SetUpQuestionResponseManager();
		//injecting 'set question' via quesetion and characterID
		DialogueManager.actionOnStartConversation.AddAction(
			(DialogueManager d) => { d.dialogueObjects = getDialogueConversation(DialogueData.INSTANCE.currentCharacterID, DialogueData.INSTANCE.currentQuestionID); });
		Debug.Log("LOG: Adding character id: " + DialogueData.INSTANCE.currentCharacterID + " and question ID to: " + DialogueData.INSTANCE.currentCharacterID);

	}


	public void SetUpQuestionResponseManager()
	{
		Debug.Log("add Questions and Response here!");
		npcToQuestionDialogueNpc.Add(1, new Dictionary<int, DialogueConversation>());

		npcToQuestionDialogueNpc[1] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 2, 3, 4}, CharacterManager.INSTANCE.GetConversationOnCharacterID(2,2)), //based on character & conversation id, return conversation
			
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});




		npcToQuestionDialogueNpc.Add(22, new Dictionary<int, DialogueConversation>());

		npcToQuestionDialogueNpc[22] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 2, 4, 5}, CharacterManager.INSTANCE.GetConversationOnCharacterID(22,2)), //based on character & conversation id, return conversation
			
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});


		npcToQuestionDialogueNpc.Add(21, new Dictionary<int, DialogueConversation>());

		npcToQuestionDialogueNpc[21] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[]{

			new (new int[] { 0,5,2 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(21, 2)),
			new(new int[] { 46 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(21, 46))

		});

		npcToQuestionDialogueNpc.Add(210, new Dictionary<int, DialogueConversation>());

		npcToQuestionDialogueNpc[210] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 2, 4, 5}, CharacterManager.INSTANCE.GetConversationOnCharacterID(210,2)), //based on character & conversation id, return conversation
			new (new int[]{46}, CharacterManager.INSTANCE.GetConversationOnCharacterID(210, 46)),
			/*
			 LOOK: here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
		//31 is the character id for ALL clues
		npcToQuestionDialogueNpc.Add(31, new Dictionary<int, DialogueConversation>());
		npcToQuestionDialogueNpc[31] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[]{
			new (new int[] { 1 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(31, 1))
		});
		npcToQuestionDialogueNpc.Add(200, new Dictionary<int, DialogueConversation>());

		//HERBIE
		npcToQuestionDialogueNpc[200] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(200,1)), //based on character & conversation id, return conversation
			new (new int[] {46}, CharacterManager.INSTANCE.GetConversationOnCharacterID(200,46)),
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
		//WERTZ
		npcToQuestionDialogueNpc.Add(201, new Dictionary<int, DialogueConversation>());
		npcToQuestionDialogueNpc[201] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(201,1)), //based on character & conversation id, return conversation
			new (new int[] {46}, CharacterManager.INSTANCE.GetConversationOnCharacterID(201,46)),
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
		npcToQuestionDialogueNpc.Add(202, new Dictionary<int, DialogueConversation>());
		npcToQuestionDialogueNpc[202] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(202,1)), //based on character & conversation id, return conversation
			new (new int[] {46}, CharacterManager.INSTANCE.GetConversationOnCharacterID(202,46)),
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
		npcToQuestionDialogueNpc.Add(203, new Dictionary<int, DialogueConversation>());
		npcToQuestionDialogueNpc[203] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(203,1)), //based on character & conversation id, return conversation
			
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
		npcToQuestionDialogueNpc.Add(204, new Dictionary<int, DialogueConversation>());
		npcToQuestionDialogueNpc[204] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(204,1)), //based on character & conversation id, return conversation
			
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
		npcToQuestionDialogueNpc.Add(205, new Dictionary<int, DialogueConversation>());
		npcToQuestionDialogueNpc[205] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(205,1)), //based on character & conversation id, return conversation
	
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
		npcToQuestionDialogueNpc.Add(206, new Dictionary<int, DialogueConversation>());
		npcToQuestionDialogueNpc[206] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(206,1)), //based on character & conversation id, return conversation
			
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
	


		/*
							//character Id
		npcToQuestionDialogueNpc.Add(3, new Dictionary<int, DialogueConversation>());
							//character Id
        npcToQuestionDialogueNpc[3] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		new(new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(3, 0)),
		
		// here, question 0 returns a conversation response of character 2, with question id 1. 
		 

		});



        //todo add npcs here
		*/


	}

	public Dictionary<int, DialogueConversation> AddQuestionsIDToCharacterAnswer((int[], DialogueConversation)[] keys)
	{
		Dictionary<int, DialogueConversation> keyValuePairs = new Dictionary<int, DialogueConversation>();

		for (int i = 0; i < keys.Length; i++)
		{

			for (int j = 0; j < keys[i].Item1.Length; j++)
			{

				keyValuePairs.Add(keys[i].Item1[j], keys[i].Item2); //for each int in key array, add a responding key pair value.
			}
		}
		return keyValuePairs;
	}


	public DialogueObject[] getDialogueConversation(int characterID, int playerQuestionID)
	{
		//code for question response type dialog
		if (!npcToQuestionDialogueNpc.ContainsKey(characterID))
		{
			throw new KeyNotFoundException("character ID " + characterID + " does not match with value");
		}
		if (!npcToQuestionDialogueNpc[characterID].ContainsKey(playerQuestionID))
		{
			Debug.Log("playerQuestion ID " + playerQuestionID + " does not match player question value for character " + characterID);
			/*
			if (!npcToQuestionDialogueNpc[characterID].ContainsKey(dialogueData.currentPersistentConversationID))
			{
				throw new KeyNotFoundException("cannot link the persistentconversationID to character" + characterID);
			}*/

			return npcToQuestionDialogueNpc[characterID][dialogueData.currentPersistentConversationID].dialogueObjects;
			//default key if no question is provided.
			//can act like a normal conversation.

		}
		Debug.Log("LOG: obtained conversation dialogue objects from character " + characterID + " based on question: " + playerQuestionID);
		return npcToQuestionDialogueNpc[characterID][playerQuestionID].dialogueObjects;

	}

    public (FileNames, JsonObject[])[] Save()
    {
        throw new NotImplementedException();
    }

    public void Load()
    {
		npcToQuestionDialogueNpc = new Dictionary<int, Dictionary<int, DialogueConversation>>();
		SetUpQuestionResponseManager();
    }
}

