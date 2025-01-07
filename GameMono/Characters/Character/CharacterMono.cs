using UnityEngine;
using System.Collections;
using System;

public class CharacterMono : BodyMono
{
	[SerializeField]
	private CharacterScriptableObject characterScriptableObject;

	[SerializeField]
	public DialogueConversation[] dialogueConversation;

	[SerializeField]
	public int persistentConversationId;
 

	public override void OnEnable()
	{
		if (characterScriptableObject != null)
		{
			bodyID = characterScriptableObject.character.ID;
			persistentConversationId = characterScriptableObject.character.persistentConversationID;
			dialogueConversation = characterScriptableObject.character.dialogueConversations;
			
		}
		gameObject.layer = LayerMask.NameToLayer("charactercollider");

    }
	public void SetCharacterScriptableObject(CharacterScriptableObject dialogueScriptableObject)
	{
		this.characterScriptableObject = dialogueScriptableObject;
	}
}

