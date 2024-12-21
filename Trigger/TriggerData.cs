using UnityEngine;
using System.Collections;
namespace Data
{
	public class TriggerData : StaticInstance<TriggerData>
	{
       
		//public BodyMono currentBodyOnTrigger { get; set; }
		public Trigger[] triggers { get; set; }
        public Trigger triggerOnTrigger { get; set; }
        public DialogueData dialogueData;

        public void Start()
        {
            triggers = FindObjectsOfType<Trigger>();
            dialogueData = FindObjectOfType<DialogueData>();
            SetUpTriggerManager();
            SetUpTriggerActionManager();
        }
        private void SetUpTriggerManager()
        {
            //set up triggerManager's Trigger Data
            TriggerManager.onStartTriggerManagerAction.AddAction((TriggerManager t) => { t.triggerData = this; t.triggers = triggers; });
        }
        private void SetUpTriggerActionManager()
        {
            TriggerActionManager.onTriggerActionTriggerActionManager.AddAction((TriggerActionManager t) => {t.triggerOnTrigger = this.triggerOnTrigger;});
        }
        


    }
}

