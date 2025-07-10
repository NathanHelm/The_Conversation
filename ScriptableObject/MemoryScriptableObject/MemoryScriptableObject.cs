using System;
using System.Collections;
using System.Collections.Generic;
using MemorySpawn;
using UnityEngine;
[CreateAssetMenu(fileName = "MemorySO", menuName = "ScriptableObjects/MemorySO", order = 9)]
public class MemoryScriptableObject : ScriptableObject
{
    public int characterID;
    [Header("memory ids.")]
    [SerializeField]
    public MemoryObject[] memoryIds;
 
}