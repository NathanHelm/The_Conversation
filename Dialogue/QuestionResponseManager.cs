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
	[SerializeField]
	private CharacterMono[] characters;
	private int currentID;




	//Dictionary<int, DialogueConversation> playerIDToDialogQuestions = new Dictionary<int, DialogueConversation>();
	Dictionary<int, Dictionary<int, DialogueConversation>> npcToQuestionDialogueNpc = new Dictionary<int, Dictionary<int, DialogueConversation>>();

    public override void OnEnable()
    {
		GameEventManager.INSTANCE.AddEventFunc<int, int, DialogueConversation>("getdialogueconversation", getDialogueConversation); 
		SetUpQuestionResponseManager();
		base.OnEnable();
	}
    public void Start()
    {
		dialogueData = GameEventManager.INSTANCE.OnEventFunc<DialogueData>(typeof(DialogueData).ToString().ToLower());
		Debug.Log(typeof(DialogueData).ToString().ToLower());
    }


    public void SetUpQuestionResponseManager()
	{
		//all code is preprocessed
							//character id. 
		npcToQuestionDialogueNpc.Add(1, new Dictionary<int, DialogueConversation>());

		npcToQuestionDialogueNpc[1] = AddQuestionsIDToCharacterAnswer(new (int[], DialogueConversation)[] {

		//key pair value code goes here.
						  //question id							
			new (new int[] { 2, 3, 4}, characters[0].dialogueConversation[0]), //todo make a dictionary where based on conversation id,
																			   //return conversation.
			
			

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
                if (keys[i].Item1[j] == 1 && keyValuePairs.ContainsKey(1)) //keys cannot be zero since persistent keys are label as 1.
										   //adding a key=1 will override the default persistent key.
                {
					Debug.LogError("invalid key " + keys[i].Item1[j]);
					continue;
                }

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
			Debug.Log("playerQuestion ID " + playerQuestionID + " does not match player question value for character " + characterID);

			if (!npcToQuestionDialogueNpc[characterID].ContainsKey(dialogueData.persistentConversationID))
			{
				throw new KeyNotFoundException("cannot link the persistentconversationID to character" + characterID);
			}

			return npcToQuestionDialogueNpc[characterID][dialogueData.persistentConversationID];
			//default key if no question is provided.
			//can act like a normal conversation.

		}
		return npcToQuestionDialogueNpc[characterID][playerQuestionID];
		
	}


	
}

