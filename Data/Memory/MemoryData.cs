using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MemorySpawn;
using PlasticGui.WorkspaceWindow;
using UnityEngine;
namespace Data
{
    public class MemoryData : StaticInstanceData<MemoryData>, IExecution
    {
        public MemoryObject[] memoryObjects { get; set; }
        public (MemorySpawnObject,GameObject)[] spawnedMemoryStageObject { get; set; }
        public (SubMemorySpawnObject, GameObject, GameObject)[] spawnedMemorySubStageObject { get; set; }

        public GameObject[] spawnedSubMemoryObjects { get; set; } //objects that have spawned...
        public int recentUnlockedMemoryId { get; set; } = 0; //the current memory that has been unlocked
        public int currentCharacterID { get; set; } = 0;

        public MemoryTransformationHandler memoryTransformationHandler { get; set; }

        public override void m_OnEnable()
        {
            memoryTransformationHandler = FindObjectOfType<MemoryTransformationHandler>().GetComponent<MemoryTransformationHandler>();
            memoryTransformationHandler.onEnableTransformations.AddAction(mth => { mth.currentCharacterID = currentCharacterID; mth.spawnedStages = spawnedMemoryStageObject.ToList(); });
        }


    }
}