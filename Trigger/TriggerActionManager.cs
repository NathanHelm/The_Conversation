using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
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
        SetTriggerManager();
        base.m_Start();
    }
	private void SetTriggerManager()
	{
		//set trigger manager with proper trigger actions.
        TriggerManager.onStartTriggerManagerAction.AddAction((TriggerManager t) => { t.triggerActionManager = this; });
    }

    public void SetUpTriggerActionManager()
	{
		//recall that IDs can be anything it doesn't nessecarily have to pertain to a character
		//imagine a door. A door isn't nessecary a character but with a unique id (say door ids starts with a 3) it can be used like a door.

		/*
		 *  BODY STARTS WITH A -- 1
		 *  CHARACTERS START WITH A -- 2
		 *  SWITCH Scene START WITH A -- 3
		 */


		characterIDToTriggerAction.Add(21, () => {

        
            GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));


			
		});
        characterIDToTriggerAction.Add(20, () => {

            //note that 'trigger default' runs characterIDToTriggerAction[22] to start conversation
            Debug.Log("this character has no action-- running default conversation state. Attach some character ID functionality @ triggeraction manager");

            GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));



        });
		characterIDToTriggerAction.Add(21, ()=>{

			GameEventManager.INSTANCE.OnEvent(typeof(ImmediateConversationState));

		});

        characterIDToTriggerAction.Add(12, () =>
		{
			//Game
		});

		characterIDToTriggerAction.Add(31, ()=>{

			
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

		if (!characterIDToTriggerAction.ContainsKey(characterID))
		{
			int n = (int)Mathf.Pow(10,characterID.ToString().Length -1);
			int getFirstVal = characterID;
			if(n < characterID)
			{
				getFirstVal = characterID / n;
			}
			//if getfirstvalue = 2, character,
			//if 1, its a body. 
			if (triggerOnTrigger.charactersOnTrigger.Count > 0 && getFirstVal == 2)
			{
				if (triggerOnTrigger.charactersOnTrigger[0] is CharacterMono)
				{
					//enable default conversation
					return characterIDToTriggerAction[20]; //writes message saying 'running default event...'
				}
			}

			else if (triggerOnTrigger.bodiesOnTrigger.Count > 0 && getFirstVal == 1)
			{

				return () => { Debug.Log("this is a body-- attach some character ID functionality @ triggeraction manager"); };
			}
			else
			{
				return ()=>{Debug.Log("neither body nor character. \nPlaying it safe and running no trigger action.");};
			}
		}
		 
			return characterIDToTriggerAction[characterID];
		 
	}

	 

}

