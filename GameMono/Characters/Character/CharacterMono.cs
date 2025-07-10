using UnityEngine;
using System.Collections;
using System;

public class CharacterMono : BodyMono
{
	[SerializeField]
	private CharacterScriptableObject characterScriptableObject;
	[SerializeField]
	private MemoryScriptableObject memoryScriptableObject;

	public MemoryObject[] memoryObjects { get; set; }
	public DialogueConversation[] dialogueConversation { get; set; }

	public Texture[] characterFaceSheet {get; set;}

	[SerializeField]
	public int persistentConversationId;
 

	public override void OnEnable()
	{
		if (characterScriptableObject != null)
		{
			bodyID = characterScriptableObject.character.ID;
			persistentConversationId = characterScriptableObject.character.persistentConversationID;
			dialogueConversation = characterScriptableObject.character.dialogueConversations;
			characterFaceSheet = characterScriptableObject.character.characterFaceSheet;
		}
		else
		{
			Debug.Log("Character doesn't have character scriptable object.");
		}
		if (memoryScriptableObject != null)
		{
			memoryObjects = memoryScriptableObject.memoryIds;
		}
		gameObject.layer = LayerMask.NameToLayer("charactercollider");

    }
	public void SetCharacterScriptableObject(CharacterScriptableObject dialogueScriptableObject)
	{
		this.characterScriptableObject = dialogueScriptableObject;
	}
}

