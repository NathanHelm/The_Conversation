using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Data;
public class QuestionResponseManager : StaticInstance<QuestionResponseManager>
{
	//based on relevant Character and Question id; output a conversation response.

	private DialogueData dialogueData;
	
	Dictionary<int, Dictionary<int, DialogueConversation>> npcToQuestionDialogueNpc = new Dictionary<int, Dictionary<int, DialogueConversation>>();

    public override void OnEnable()
    {
		MManager.onStartManagersAction.AddAction((MManager m) =>
		{
			m.questionResponseManager = this;
		});

        base.OnEnable();

	}

    public override void m_Start()
    {

        SetUpQuestionResponseManager();
		//injecting 'set question' via quesetion and characterID
		DialogueManager.actionOnStartConversation.AddAction((DialogueManager d) => { d.dialogueObjects = getDialogueConversation(DialogueData.INSTANCE.currentCharacterID, DialogueData.INSTANCE.currentQuestionID); });
        Debug.Log(typeof(DialogueData).ToString().ToLower());
		
    }


    public void SetUpQuestionResponseManager()
	{
   
        npcToQuestionDialogueNpc.Add(1, new Dictionary<int, DialogueConversation>());

		npcToQuestionDialogueNpc[1] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
		//question id							
			new (new int[] { 2, 3, 4}, CharacterManager.INSTANCE.GetConversationOnCharacterID(1,2)), //based on character & conversation id, return conversation
			
			/*
			 here, questions 2, 3, and 4, return a conversation response of character 1, with question id 2. 
			 */
		});
							//character Id
		npcToQuestionDialogueNpc.Add(3, new Dictionary<int, DialogueConversation>());
							//character Id
        npcToQuestionDialogueNpc[3] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		new(new int[] { 0 }, CharacterManager.INSTANCE.GetConversationOnCharacterID(3, 0)),
		/*
		 here, question 0 returns a conversation response of character 2, with question id 1. 
		 */

		});

        //todo add npcs here


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


    public DialogueObject[] getDialogueConversation(int characterID, int playerQuestionID)
	{
		//code for question response type dialog
		if(!npcToQuestionDialogueNpc.ContainsKey(characterID))
		{
			throw new KeyNotFoundException("character ID "+ characterID + " does not match with value");
		}
		if (!npcToQuestionDialogueNpc[characterID].ContainsKey(playerQuestionID))
		{
			Debug.Log("playerQuestion ID " + playerQuestionID + " does not match player question value for character " + characterID);

			if (!npcToQuestionDialogueNpc[characterID].ContainsKey(dialogueData.currentPersistentConversationID))
			{
				throw new KeyNotFoundException("cannot link the persistentconversationID to character" + characterID);
			}

			return npcToQuestionDialogueNpc[characterID][dialogueData.currentPersistentConversationID].dialogueObjects;
			//default key if no question is provided.
			//can act like a normal conversation.

		}
		return npcToQuestionDialogueNpc[characterID][playerQuestionID].dialogueObjects;
		
	}
 
}

