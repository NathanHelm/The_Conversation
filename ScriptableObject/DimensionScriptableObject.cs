using UnityEngine;
using System.Collections;
using Cinemachine;
[CreateAssetMenu(fileName = "DimensionSO", menuName = "ScriptableObjects/DimensionSO", order = 1)]
public class DimensionScriptableObject : ScriptableObject
{
	// Use this for initialization
	public CinemachineVirtualCamera cinemachineVirtualCamera; //use cinemachine vc

	public string playanimationclip;
	//spawn hands? 
	public bool isOrthographic;
}

