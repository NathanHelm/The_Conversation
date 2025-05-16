using UnityEngine;
using System.Collections;
using System;

public class CharacterMono : BodyMono
{
	[SerializeField]
	private CharacterScriptableObject characterScriptableObject;


	public DialogueConversation[] dialogueConversation {get; set;}
	public MemoryStage[] memoryStages {get; set;}

	[SerializeField]
	public int persistentConversationId;
 

	public override void OnEnable()
	{
		if (characterScriptableObject != null)
		{
			bodyID = characterScriptableObject.character.ID;
			persistentConversationId = characterScriptableObject.character.persistentConversationID;
			dialogueConversation = characterScriptableObject.character.dialogueConversations;
			memoryStages = characterScriptableObject.character.memoryStages;
		}
		else
		{
			Debug.Log("Character doesn't have character scriptable object.");
		}
		gameObject.layer = LayerMask.NameToLayer("charactercollider");

    }
	public void SetCharacterScriptableObject(CharacterScriptableObject dialogueScriptableObject)
	{
		this.characterScriptableObject = dialogueScriptableObject;
	}
}

