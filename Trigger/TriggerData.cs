using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Data
{
	public class TriggerData : StaticInstance<TriggerData>, IExecution
	{
       

		public Trigger[] triggers { get; set; }

        public List<CharacterMono> characterMonosInTrigger = new(); //using for selection conversation state.
        public Trigger triggerOnTrigger { get; set; }
        public DialogueData dialogueData;

        public bool isPlayerOnCharacter {get; set;} = false;

        public override void m_OnEnable()
        {
            triggers = FindObjectsOfType<Trigger>();
            dialogueData = FindObjectOfType<DialogueData>();
            SetUpTriggerManager();
            SetUpTriggerActionManager();
            
            TriggerManager.onStartTriggerManagerAction.AddAction((TriggerManager t) => { t.triggerActionManager = TriggerActionManager.INSTANCE; });
            
            
        }
        private void SetUpTriggerManager()
        {
            //DO NOT null check static instance, it is expected that instance is null!!!
            TriggerManager.onStartTriggerManagerAction.AddAction((TriggerManager t) => { t.triggerData = this; t.triggers = triggers; });
        }
        private void SetUpTriggerActionManager()
        {
           
            TriggerActionManager.onTriggerActionTriggerActionManager.AddAction((TriggerActionManager t) => { t.triggerOnTrigger = this.triggerOnTrigger; });
        }
        


    }
}

