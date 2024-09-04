using UnityEngine;
using System.Collections;

namespace Data
{

	public class DialogueData : StaticInstance<DialogueData>
	{
		[SerializeField]
		public int characterID { get; set; }
		[SerializeField]
		public int questionID { get; set; }
		[SerializeField]
		public DialogueScriptableObject player;
		[SerializeField]
		public DialogueScriptableObject currentNPC = null;
		public DialogueConversation currentConversation;
		public DialogueManager dialogueManager { get; set; }
		public QuestionResponseManager questionResponseManager { get; set; }
		public UIManager uIManager;

		private void Start()
		{
			//manager, provide all logic in its class. 
			dialogueManager = GameEventManager.INSTANCE.OnEventFunc<DialogueManager>(typeof(DialogueManager).ToString().ToLower());
			questionResponseManager = GameEventManager.INSTANCE.OnEventFunc<QuestionResponseManager>(typeof(QuestionResponseManager).ToString().ToLower());
			uIManager = GameEventManager.INSTANCE.OnEventFunc<UIManager>(typeof(UIManager).ToString().ToLower());
		}




	}
}

