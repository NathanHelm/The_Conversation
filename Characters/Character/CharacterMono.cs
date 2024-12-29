using UnityEngine;
using System.Collections;
using System;

public class CharacterMono : BodyMono
{
	[SerializeField]
	private DialogueScriptableObject dialogueScriptableObject;

	[SerializeField]
	public DialogueConversation[] dialogueConversation;

	[SerializeField]
	public int persistentConversationId;
 

	public override void OnEnable()
	{
		if (dialogueScriptableObject != null)
		{
			//			throw new NullReferenceException("scriptable object not attached to --> " + gameObject.name); 

			bodyID = dialogueScriptableObject.character.ID;
			persistentConversationId = dialogueScriptableObject.character.persistentConversationID;
			dialogueConversation = dialogueScriptableObject.character.dialogueConversations;
		}
	}
	public void SetDialogueScriptableObject(DialogueScriptableObject dialogueScriptableObject)
	{
		this.dialogueScriptableObject = dialogueScriptableObject;
	}
}

