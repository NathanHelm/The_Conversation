using UnityEngine;
using System.Collections;

public class CharacterMono : BodyMono
{
	[SerializeField]
	DialogueScriptableObject dialogueScriptableObject;

	public DialogueConversation[] dialogueConversation { get; set; }

	public int persistentConversationId;
 

	public override void OnEnable()
	{
		bodyID = dialogueScriptableObject.character.ID;
		dialogueConversation = dialogueScriptableObject.character.dialogueConversations;
	}
}

