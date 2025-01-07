using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/CharacterSO", order = 2)]
public class CharacterScriptableObject : ScriptableObject
{
	[SerializeField]
	public Character character;
}

