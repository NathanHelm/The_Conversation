using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
using System.Linq;

public class TriggerActionManager : StaticInstance<TriggerActionManager>, IExecution
{
	public static SystemActionCall<TriggerActionManager> onTriggerActionTriggerActionManager = new SystemActionCall<TriggerActionManager>();
	public Dictionary<int, Action> characterIDToTriggerAction = new Dictionary<int, Action>();
	public Dictionary<int, Action> characterIDToTriggerExitAction = new Dictionary<int, Action>();
	public Trigger triggerOnTrigger { get; set; }

    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) => { m.triggerActionManager = this; });
        base.m_OnEnable();
    }
    public override void m_Start()
    {
		SetUpTriggerActionManager();
        base.m_Start();
    }
	public override void OnDisable()
    {
        base.OnDisable();
    }

	public void SetUpTriggerActionManager()
	{
		//recall that IDs can be anything it doesn't nessecarily have to pertain to a character
		//imagine a door. A door isn't nessecary a character but with a unique id (say door ids starts with a 3) it can be used like a door.

		/*
		 *  BODY STARTS WITH A -- 1
		 *  CHARACTERS START WITH A -- 2
		 *  CLUES START WITH A -- 3
		 */


		characterIDToTriggerAction.Add(20, () =>
		{

			//note that 'trigger default' runs characterIDToTriggerAction[22] to start conversation
			Debug.Log("this character has no action-- running default conversation state. Attach some character ID functionality @ triggeraction manager");
			GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));

		});

		characterIDToTriggerAction.Add(2000, () =>
		{
			Debug.Log("SELECT-ENTERED2TRIGGERS");
			//edge case: checks whether runenddialogueselection exists to prevent stacking of actions if trigger hits objects > 2
			if (!ActionController.INSTANCE.DoesActionExist(ActionController.AFTERDIALOGUE, ActionController.INSTANCE.actionEndDialogue.runEndDialogueSelectionAgain))
			{
				ActionController.AFTERDIALOGUE += ActionController.INSTANCE.actionEndDialogue.runEndDialogueSelectionAgain;
			}
			GameEventManager.INSTANCE.OnEvent(typeof(StartSelectionConversationState));

		});
		characterIDToTriggerAction.Add(2001, () =>
		{
			//immediate dialogue!
			GameEventManager.INSTANCE.OnEvent(typeof(ImmediateConversationState));


		});


		//NOTE: EVERY CHARACTER- or characterID with the id 2 - will run the action below, use with caution!
		characterIDToTriggerAction.Add(2, () =>
		{
			Debug.Log("hey hey, its me number 2!");
			//above, if user pressed tab on a character they are taken to the character's interview state.
			GameEventManager.INSTANCE.OnEvent(typeof(InterviewLedgerState));
			GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));

			//one change that will effect all chracters 

		});
		characterIDToTriggerAction.Add(12, () =>
	   {

		   //Game
	   });

		//exit trigger========================================================================================
		characterIDToTriggerExitAction.Add(2, () =>
		{
			DialogueManager.INSTANCE.NoDialogue();
			GameEventManager.INSTANCE.OnEvent(typeof(NoConversationState));
			GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));

		});
		characterIDToTriggerExitAction.Add(2000, () =>
		{
			Debug.Log("SELECT-EXITED2TRIGGERS");
			if (ActionController.INSTANCE.DoesActionExist(ActionController.AFTERDIALOGUE, ActionController.INSTANCE.actionEndDialogue.runEndDialogueSelectionAgain))
			{
				ActionController.AFTERDIALOGUE -= ActionController.INSTANCE.actionEndDialogue.runEndDialogueSelectionAgain;
			}
			DialogueManager.INSTANCE.NoDialogue();
			GameEventManager.INSTANCE.OnEvent(typeof(NoConversationState));
			GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
		});
 

       

		
		/*

		characterIDToTriggerAction.Add(2, ()=>{
			
			Debug.Log("IMMEDIATE DIALOG TESTING!");
			GameEventManager.INSTANCE.OnEvent(typeof(ImmediateConversationState));
		});
		*/

	}
	public void PlayTriggerAction(int actionId)
	{
		if(!characterIDToTriggerAction.ContainsKey(actionId))
		{
			Debug.Log("action trigger " + actionId + " could not be found.");
			return;
		}
		Debug.Log("action id" + actionId + " found and running!");
	    characterIDToTriggerAction[actionId]();
	}
	/*
	I fkn hate this code below, 
	I think I made the mistake on over relying on integer ids which suck if you want multiple paticular events to kick off because of you id... 
	
	*/

	public Action GetTriggerActionMultipleTriggers(int[] charactersInTrigger)
	{

		foreach (var characterID in charactersInTrigger)
		{
			//first check if any trigger is unique to the 2 -- conversation state
			if (characterIDToTriggerAction.ContainsKey(characterID))
			{
				return () => { Debug.LogError("There are two triggers. However one is a unique trigger so we will run the first unique trigger."); };
			}
			if (GetFirstVal(characterID) != 2)
			{
				return () => { Debug.LogWarning("multiple triggers are not characters"); };
			}
		}
		return characterIDToTriggerAction[2000];

	}
	public Action GetTriggerExitActionMultipleTriggers(int[] charactersInTrigger, CharacterMono characterNotOnTrigger)
	{

		foreach (var characterID in charactersInTrigger)
		{
			//first check if any trigger is unique to the 2 -- conversation state
			if (characterIDToTriggerExitAction.ContainsKey(characterID))
			{
				return () => { Debug.LogError("There are two exit triggers. However one is a unique trigger so we will run the first unique trigger exit."); };
			}
			if (charactersInTrigger.Length == 2)
			{
				//bullshit
				var dialogueData = DialogueData.INSTANCE;
				dialogueData.currentCharacterID = characterNotOnTrigger.bodyID;
				dialogueData.currentPersistentConversationID = characterNotOnTrigger.persistentConversationQuestionId;
				return characterIDToTriggerAction[2];
			}

			if (charactersInTrigger.Length > 2)
				{
					return () => { Debug.LogError("not running exit because characters in trigger > 0 length is" + charactersInTrigger.Length); };
				}
			if (GetFirstVal(characterID) != 2)
				{
					return () => { Debug.LogWarning("multiple triggers are not characters"); };
				}
		}
		
		return characterIDToTriggerExitAction[2000];

	}
 


	public Action GetTriggerAction(int characterID) //really only used for ontrigger events :/ 
	{
		onTriggerActionTriggerActionManager?.RunAction(this);

		int getFirstVal = GetFirstVal(characterID);
		Debug.Log("LOG first value that's in trigger --> " + getFirstVal);

		if (!characterIDToTriggerAction.ContainsKey(characterID)) //if there is no character id found the warrant a trigger event (see characterIDToTriggerAction dictionary)
		{
			Debug.LogError("could not find id " + characterID + " in trigger action");
		}
		if (getFirstVal == 2)
		{
			Debug.Log("LOG playing default character trigger at --> 2");
			characterIDToTriggerAction[2]();
		}
		else if (getFirstVal == 3)
		{
			characterIDToTriggerAction[3]();
		}
		else if (getFirstVal == 1)
		{
			characterIDToTriggerAction[1]();
		}
		else
		{
			Debug.LogWarning("interesting -- your id with: " + getFirstVal + " does not start with 1, 2, or 3.");
		}

		if (!characterIDToTriggerAction.ContainsKey(characterID)) //if there is no character id found the warrant a trigger event (see characterIDToTriggerAction dictionary)
		{
			Debug.LogError("could not find id " + characterID + " in trigger exit action");
			return () => { Debug.LogError("run action!"); };
		}


		return characterIDToTriggerAction[characterID];

	}
	public Action GetTriggerExitAction(int characterID) //really only used for ontrigger events :/ 
	{
		onTriggerActionTriggerActionManager?.RunAction(this);

		int getFirstVal = GetFirstVal(characterID);
		Debug.Log("LOG first value that's in trigger --> " + getFirstVal);

		if (getFirstVal == 1)
		{
			characterIDToTriggerExitAction[1]();
		}
		else if (getFirstVal == 2)
		{
			Debug.Log("LOG playing default character trigger at --> 2");
			characterIDToTriggerExitAction[2]();
		}
		else if (getFirstVal == 3)
		{
			characterIDToTriggerExitAction[3]();
		}
		else
		{
			Debug.LogWarning("interesting -- your id with: " + getFirstVal + " does not start with 1, 2, or 3.");
		}

		if (!characterIDToTriggerExitAction.ContainsKey(characterID)) //if there is no character id found the warrant a trigger event (see characterIDToTriggerAction dictionary)
		{
			Debug.LogError("could not find id " + characterID + " in trigger exit action");
			return () => { Debug.LogError("run action!"); };
		}


		

		return characterIDToTriggerExitAction[characterID];

	}


	private int GetFirstVal(int characterID)
	{
		return int.Parse(characterID.ToString()[0].ToString());
	}
	private int GetNVal(int characterID, int N)
	{
		return int.Parse(characterID.ToString()[N].ToString());
	}

	 

}

