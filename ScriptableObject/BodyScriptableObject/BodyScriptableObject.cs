using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "bodyID", menuName = "ScriptableObjects/bodyID", order = 3)]
public class BodyScriptableObject : ScriptableObject
{
    [SerializeField]
    public int bodyID;
}

