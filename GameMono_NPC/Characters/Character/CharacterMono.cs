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
	public int persistentConversationQuestionId;

	[SerializeField]
	public string interviewFaceStreamingAssetsImageName;

    public override void OnEnable()
	{

		if (characterScriptableObject != null)
		{
			bodyName = characterScriptableObject.character.bodyName;
			bodyID = characterScriptableObject.character.ID;
			persistentConversationQuestionId = characterScriptableObject.character.persistentConversationID;
			dialogueConversation = characterScriptableObject.character.dialogueConversations;
			characterFaceSheet = characterScriptableObject.character.characterFaceSheet;
			interviewFaceStreamingAssetsImageName = characterScriptableObject.character.interviewTexture;
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

