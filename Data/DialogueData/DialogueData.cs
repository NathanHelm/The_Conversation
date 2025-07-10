using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Data
{

	public class DialogueData : StaticInstanceData<DialogueData>, IExecution
	{
		[SerializeField]
		public int currentCharacterID, currentPersistentConversationID;
		[SerializeField]
		public int currentQuestionID { get; set; }

		//TODO make it so that when we are on interview scene we run previoussceneinterivewlederstate (its open ledger except when we press tab, )
		
		//public DialogueManager dialogueManager { get; set; }
		//public QuestionResponseManager questionResponseManager { get; set; }
		//public UIManager uIManager;
		//public CharacterManager characterManager;

		public void Start()
		{
			//manager, provide all logic in its class. 
			//dialogueManager = GameEventManager.INSTANCE.OnEventFunc<DialogueManager>(typeof(DialogueManager).ToString().ToLower());
			//questionResponseManager = GameEventManager.INSTANCE.OnEventFunc<QuestionResponseManager>(typeof(QuestionResponseManager).ToString().ToLower());
			//uIManager = GameEventManager.INSTANCE.OnEventFunc<UIManager>(typeof(UIManager).ToString().ToLower());
			//characterManager = GameEventManager.INSTANCE.OnEventFunc<CharacterManager>(typeof(CharacterManager).ToString().ToLower());

			//SetDialogueData();

		}
		public override void m_OnEnable()
		{

			//SetDialogueData();
			  SetTriggerManager();
           
        }

        public override void m_Start()
        {
		   //make sure that priority actions are used before the start function. 
         
        }
		

		private void SetTriggerManager()
		{
			TriggerManager.onStartTriggerManagerAction.AddAction((TriggerManager t) => { t.dialogueData = this; });

		}
		

      
    }
}

