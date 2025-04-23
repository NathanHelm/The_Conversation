using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClueSO", menuName = "ScriptableObjects/ClueSO", order = 4)]
public class ClueScriptableObject : ScriptableObject
{
   	public string imageDescription;
	public int questionID; //certain images unlock a question that vet can ask to others.
    public int[] memoryId;
    public Texture ledgerImage;

    public Texture[] ledgerOverlays;

    public DialogueConversation dialogConversation;
    
}
