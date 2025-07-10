using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Persistence;
using System;
using JetBrains.Annotations;
using System.Linq;
using MemorySpawn;
using Data;


public class MemorySpawnerManager : StaticInstance<MemorySpawnerManager>, ISaveLoad, IExecution
{
    public Subject<ObserverAction.MemorySpawnerAction> subject { get; set; } = new(); 
    public List<MemorySpawnObject> spawnedMemoryStageObject { get; set; } = new();
    public List<int> spawnedMemoryID { get; set; } = new();

    private Dictionary<int, Dictionary<int, Dictionary<int, GameObject>>> idToMemorySpawnSubGameobject = new();
    private Dictionary<int, Dictionary<int, MemorySpawnObject>> idToMemorySpawnGameobject = new();

    [Header("add all clues here")]
    [SerializeField]
    private MemorySpawnScriptableObject[] memoryScriptableObjects;

    public override void m_Start()
    {
        base.m_Start();
    }
    public override void m_OnEnable()
    {
         MManager.INSTANCE.onStartManagersAction.AddAction(mm => { mm.memorySpawnerManager = this; });
    }
    public void Update()
    {
        subject.NotifyObservers(ObserverAction.MemorySpawnerAction.onTransformObjectUpdate);
    }

    public void PopulateMemoryDictionary()
    {
        //getting all memory and submemory gamobjects from the scriptable object...
        if (idToMemorySpawnGameobject.Count > 0)
        {
            Debug.LogError("memory stage objects are already initialized to dictionary");
            return;
        }
        foreach (var single in memoryScriptableObjects)
            {
                if (!idToMemorySpawnSubGameobject.ContainsKey(single.characterId))
                {
                    idToMemorySpawnGameobject.Add(single.characterId, new Dictionary<int, MemorySpawnObject>());
                    idToMemorySpawnSubGameobject.Add(single.characterId, new());
                }
                foreach (var single1 in single.memorySpawnObjects)
                {
                    idToMemorySpawnGameobject[single.characterId].Add(single1.memoryId, single1);

                    idToMemorySpawnSubGameobject[single.characterId].Add(single1.memoryId, new());

                    foreach (var single2 in single1.subMemories)
                    {
                        idToMemorySpawnSubGameobject[single.characterId][single1.memoryId].Add(single2.id, single2.subMemoryObject);
                    }

                }
            }
    }

    public GameObject CreateMemoryStage(GameObject memoryStage)
    {
        return Instantiate(memoryStage, GameObject.FindGameObjectWithTag("MemoryParent").transform);
    }
    public void SpawnStageOnID(int characterID, int memoryID)
    {
        if (!idToMemorySpawnGameobject.ContainsKey(characterID))
        {
            Debug.LogError("character Id " + characterID +
            " not found for spawn stages.");
            
        }
        
        var memoryDict = idToMemorySpawnGameobject[characterID];

        MemorySpawnObject memoryStageObject = memoryDict[memoryID];

        spawnedMemoryStageObject.Add(new MemorySpawnObject(Vector2.zero,memoryStageObject.characterId,memoryStageObject.memoryId, CreateMemoryStage(memoryStageObject.memoryGameObject), memoryStageObject.memoryPos, memoryStageObject.subMemories));

    }
    //TODO add this method with correct logic. 
    public void SpawnSubStageOnID(int characterID, int memoryID, int subMemoryId)
    {
        var subMemoryDict = idToMemorySpawnSubGameobject[characterID][memoryID];

        GameObject subMemoryStage = subMemoryDict[subMemoryId];

        CreateMemoryStage(subMemoryStage);
    }

    public void Load()
    {
        List<JsonUnlockMemoryObject> jsonUnlockMemoryObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonUnlockMemoryObject>(FileNames.UnlockedMemoryFile);


        jsonUnlockMemoryObjects.ForEach(character =>
        {
            foreach (var memoryObject in character.memoryObjects)
            {
                SpawnStageOnID(character.characterID, memoryObject.ID);
            }
            MemoryData.INSTANCE.memorySpawnObjects = spawnedMemoryStageObject.ToArray();
            MemoryData.INSTANCE.currentCharacterID = character.characterID;
        });

        subject.NotifyObservers(ObserverAction.MemorySpawnerAction.onAfterStageObjectsSpawn);

    }

    public (FileNames, JsonObject[])[] Save()
    {
        throw new System.NotImplementedException();
    }
   

  
}