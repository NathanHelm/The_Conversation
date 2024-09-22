using UnityEngine;
using System.Collections;

public class CharacterMono : BodyMono
{
	[SerializeField]
	DialogueScriptableObject dialogueScriptableObject;

	[SerializeField]
	public DialogueConversation[] dialogueConversation;

	[SerializeField]
	public int persistentConversationId;
 

	public override void OnEnable()
	{
		bodyID = dialogueScriptableObject.character.ID;
		persistentConversationId = dialogueScriptableObject.character.persistentConversationID;
		dialogueConversation = dialogueScriptableObject.character.dialogueConversations;
	}
}

