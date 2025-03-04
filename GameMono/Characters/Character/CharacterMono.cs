using UnityEngine;
using System.Collections;
using System;

public class CharacterMono : BodyMono
{
	[SerializeField]
	private CharacterScriptableObject characterScriptableObject;

	[SerializeField]
	private MemoryScriptableObject memoryScriptableObject;

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
			
		}
		else
		{
			Debug.Log("Character doesn't have character scriptable object.");
		}
		if(memoryScriptableObject != null)
		{
			this.memoryStages = memoryScriptableObject.memoryStage;
		}
		else
		{
			Debug.Log("Character doesn't have memeory scriptable object.");
		}
		gameObject.layer = LayerMask.NameToLayer("charactercollider");

    }
	public void SetCharacterScriptableObject(CharacterScriptableObject dialogueScriptableObject)
	{
		this.characterScriptableObject = dialogueScriptableObject;
	}
}

