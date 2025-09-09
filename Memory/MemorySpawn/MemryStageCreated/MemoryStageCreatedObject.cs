using System.Collections.Generic;
using MemorySpawn;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class MemoryStageCreatedObject
{
    //NOTE: this object is different from memory spawn object. 
    //memory spawn object is used for the scriptable object class. Memory stage created object is not. 
    //when our object is been created, it is stored in one of these object to be used in other classes (transform handler being the notable one for now)
    public GameObject spawnedMemoryStage { get; set; }
    public List<SubMemoryStageCreatedObject> spawnedSubMemory = new();

    [Header("offset postion from on enable transformation")]
    public Vector3 enableOffsetPostion = Vector3.zero;

    public MemorySpawnObject memorySpawnObject;

    public MemoryStageCreatedObject(GameObject memoryStage, MemorySpawnObject memorySpawnObject)
    {
        this.spawnedMemoryStage = memoryStage;
        this.memorySpawnObject = memorySpawnObject;
    }
}