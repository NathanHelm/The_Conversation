using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "handSO", menuName = "ScriptableObjects/handSO", order = 10)]
public class HandScriptableObject : ScriptableObject
{
    public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;

    public Vector2 farthestLeft;

    public Vector2 farthestRight;

    public Vector2 leftPageObjectPosition;
    public Vector2 rightPageObjectPosition;

}   