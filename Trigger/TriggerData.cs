using UnityEngine;
using System.Collections;
namespace Data
{
	public class TriggerData : StaticInstance<TriggerData>
	{
        public TriggerActionManager triggerActionManager { get; set; }
		public TriggerManager triggerManager { get; set; }
		//public BodyMono currentBodyOnTrigger { get; set; }
		public Trigger[] triggers { get; set; }
        public Trigger triggerOnTrigger { get; set; }
        public DialogueData dialogueData;

        public void Start()
        {
            triggerManager = GameEventManager.INSTANCE.OnEventFunc<TriggerManager>(typeof(TriggerManager).ToString().ToLower());
            triggers = FindObjectsOfType<Trigger>();
            dialogueData = FindObjectOfType<DialogueData>();
            triggerActionManager = GameEventManager.INSTANCE.OnEventFunc<TriggerActionManager>(typeof(TriggerActionManager).ToString().ToLower());

        }


    }
}

