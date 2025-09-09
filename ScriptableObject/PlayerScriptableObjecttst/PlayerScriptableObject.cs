using UnityEngine;
using System.Collections;
[CreateAssetMenu(fileName = "playerSO", menuName = "ScriptableObjects/playerSO", order = 1)]
public class PlayerScriptableObject : ScriptableObject
{
	public float moveSpeed;
	public float velocityThreshold;
	public float cameraSensitivity;

}

