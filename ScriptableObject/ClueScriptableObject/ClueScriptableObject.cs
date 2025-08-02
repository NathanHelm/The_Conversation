using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClueSO", menuName = "ScriptableObjects/ClueSO", order = 4)]

public class ClueScriptableObject : ScriptableObject
{
   	public string imageDescription;
    [Header("The unlocked conversation ID that can be asked to characters")]
	public int questionID; //certain images unlock a question that vet can ask to others.
    public Texture ledgerImage;
    [Header("official id for the clue.")]
    public int clueID;
    public int clueQuestionID;

    public Texture[] ledgerOverlays;

    [Header("there is no need to add a id-- you will be routed to cluemono static instance")]
    public DialogueConversation dialogConversation;


    
}
