using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/DialogueSO", order = 2)]
public class DialogueScriptableObject : ScriptableObject
{

    public DialogueConversation dialogueConversations { get; set; }
}
