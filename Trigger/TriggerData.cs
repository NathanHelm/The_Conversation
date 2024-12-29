using UnityEngine;
using System.Collections;
namespace Data
{
	public class TriggerData : StaticInstance<TriggerData>
	{
       

		public Trigger[] triggers { get; set; }
        public Trigger triggerOnTrigger { get; set; }
        public DialogueData dialogueData;

        public override void OnEnable()
        {
            triggers = FindObjectsOfType<Trigger>();
            dialogueData = FindObjectOfType<DialogueData>();
            SetUpTriggerManager();
            SetUpTriggerActionManager();
            
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

