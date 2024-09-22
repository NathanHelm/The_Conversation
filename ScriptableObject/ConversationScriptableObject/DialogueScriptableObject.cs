using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "ScriptableObjects/DialogueSO", order = 2)]
public class DialogueScriptableObject : ScriptableObject
{
	[SerializeField]
	public Character character;
}

