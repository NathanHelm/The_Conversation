using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClueSO", menuName = "ScriptableObjects/ClueSO", order = 4)]
public class ClueScriptableObject : ScriptableObject
{
    //conversation already included
    public int[] clueQuestions; //questions 'unlocked' after clicking on image.
    public string imgDescription;
    public int bodyId;
    public DialogueConversation[] dialogueConversations;
}
