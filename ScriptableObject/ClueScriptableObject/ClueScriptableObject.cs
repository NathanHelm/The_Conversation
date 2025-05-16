using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClueSO", menuName = "ScriptableObjects/ClueSO", order = 4)]

public class ClueScriptableObject : ScriptableObject
{
   	public string imageDescription;
	public int questionID; //certain images unlock a question that vet can ask to others.
    public Texture ledgerImage;

    public int clueQuestionID;

    public Texture[] ledgerOverlays;

    [Header("there is no need to add a id-- you will be routed to cluemono static instance")]
    public DialogueConversation dialogConversation;


    
}
