using UnityEngine;
using System.Collections;

namespace Data
{

	public class DialogueData : StaticInstance<DialogueData>
	{
		[SerializeField]
		public int currentCharacterID, currentPersistentConversationID;
		[SerializeField]
		public int currentQuestionID { get; set; }
		 
		[SerializeField]
		public DialogueScriptableObject player;
		[SerializeField]
		public DialogueScriptableObject currentNPC = null;
		public DialogueConversation currentConversation;
		public DialogueAction currentDialogAcrtion;


		public DialogueManager dialogueManager { get; set; }
		public QuestionResponseManager questionResponseManager { get; set; }
		public UIManager uIManager;
		public CharacterManager characterManager;

		private void Start()
		{
			//manager, provide all logic in its class. 
			dialogueManager = GameEventManager.INSTANCE.OnEventFunc<DialogueManager>(typeof(DialogueManager).ToString().ToLower());
			questionResponseManager = GameEventManager.INSTANCE.OnEventFunc<QuestionResponseManager>(typeof(QuestionResponseManager).ToString().ToLower());
			uIManager = GameEventManager.INSTANCE.OnEventFunc<UIManager>(typeof(UIManager).ToString().ToLower());
			characterManager = GameEventManager.INSTANCE.OnEventFunc<CharacterManager>(typeof(CharacterManager).ToString().ToLower());
		}




	}
}

