using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class DialogueScriptableObject : ScriptableObject
{
	[SerializeField]
	public Character character;
}

