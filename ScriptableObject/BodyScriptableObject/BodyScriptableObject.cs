using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "bodySO", menuName = "ScriptableObjects/bodySO", order = 3)]
public class BodyScriptableObject : ScriptableObject
{
    [SerializeField]
    public int bodyID;
}

