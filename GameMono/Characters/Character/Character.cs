using UnityEngine;
using System.Collections;
[System.Serializable]
public class Character : Body
{
	 
	public DialogueConversation[] dialogueConversations;
	public MemoryStage[] memoryStages;

	public int persistentConversationID; //this field is the INPUT for the conversation that does not change.
	//treat this id like a bookmark it saves a previous piece of dialog.

	


}

