using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class TriggerActionManager : StaticInstance<TriggerActionManager>
{
	public Dictionary<int, Action> characterIDToTriggerAction = new Dictionary<int, Action>();
	public Dictionary<int, Action> characterIDToTriggerExitAction = new Dictionary<int, Action>();

	public void SetUpTriggerActionManager()
	{
		//recall that IDs can be anything it doesn't nessecarily have to pertain to a character
		//imagine a door. A door isn't nessecary a character but with a unique id (say door ids starts with a 3) it can be used like a door.

		/*
		 *  BODY STARTS WITH A -- 1
		 *  CHARACTERS START WITH A -- 2
		 *  
		 */


		characterIDToTriggerAction.Add(11, () => {

			//todo add trigger action here.
			GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));


			
		});

	}
	public Action GetTriggerAction(int characterID)
	{
		if (!characterIDToTriggerAction.ContainsKey(characterID))
		{
			TriggerData trigger = GameEventManager.INSTANCE.OnEventFunc<TriggerData>("triggerdata");

			if(trigger.currentBodyOnTrigger is CharacterMono)
			{
				//enable default conversation
				return () => { GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));

					Debug.Log("this character has no action-- running default conversation state. Attach some character ID functionality @ triggeraction manager\"");
				};
			}
		 

			
			return () => { Debug.Log("this is a body-- attach some character ID functionality @ triggeraction manager"); };
		}

		return characterIDToTriggerAction[characterID];
		 
	}
	 

}

