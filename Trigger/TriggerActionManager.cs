using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
using Codice.Client.Common;
using PlasticPipe.PlasticProtocol.Messages;
public class TriggerActionManager : StaticInstance<TriggerActionManager>
{
	public static SystemActionCall<TriggerActionManager> onTriggerActionTriggerActionManager = new SystemActionCall<TriggerActionManager>();
	public Dictionary<int, Action> characterIDToTriggerAction = new Dictionary<int, Action>();
	public Dictionary<int, Action> characterIDToTriggerExitAction = new Dictionary<int, Action>();
	public Trigger triggerOnTrigger { get; set; }

    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager m) => { m.triggerActionManager = this; });
        base.OnEnable();
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


		characterIDToTriggerAction.Add(21, () => {

        
            GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));


			
		});
        characterIDToTriggerAction.Add(20, () => {

            //note that 'trigger default' runs characterIDToTriggerAction[22] to start conversation
            Debug.Log("this character has no action-- running default conversation state. Attach some character ID functionality @ triggeraction manager");

            GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));



        });
		characterIDToTriggerAction.Add(22, ()=>{

			GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));


		});
		//NOTE: EVERY CHARACTER- or characterID with the id 2 - will run the action below, use with caution!
		characterIDToTriggerAction.Add(2, ()=>
		{
			Debug.Log("hey hey, its me number 2!");
			GameEventManager.INSTANCE.OnEvent(typeof(InterviewLedgerState));
			GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));

			//one change that will effect all chracters 

		});
		characterIDToTriggerExitAction.Add(2, () =>
		{
			DialogueManager.INSTANCE.NoDialogue();
            GameEventManager.INSTANCE.OnEvent(typeof(NoConversationState));
			GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
			
		});

        characterIDToTriggerAction.Add(12, () =>
		{
			
			//Game
		});

		
		/*

		characterIDToTriggerAction.Add(2, ()=>{
			
			Debug.Log("IMMEDIATE DIALOG TESTING!");
			GameEventManager.INSTANCE.OnEvent(typeof(ImmediateConversationState));
		});
		*/

	}
	public void PlayAction(int actionId)
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
 


	public Action GetTriggerAction(int characterID) //really only used for ontrigger events :/ 
	{
		onTriggerActionTriggerActionManager?.RunAction(this);

		int getFirstVal = GetFirstVal(characterID);
		Debug.Log("LOG first value that's in trigger --> " + getFirstVal);

		if (!characterIDToTriggerAction.ContainsKey(characterID)) //if there is no character id found the warrant a trigger event (see characterIDToTriggerAction dictionary)
		{
			Debug.LogError("could not find id " + characterID +" in trigger action");
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

