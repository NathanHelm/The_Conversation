using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Persistence;
using System;
using JetBrains.Annotations;
using System.Linq;
using MemorySpawn;
using Data;
using ObserverAction;



public class MemorySpawnerManager : StaticInstance<MemorySpawnerManager>, ISaveLoad, IExecution, IObserver<ObserverAction.MemoryTransform>
{
    public Subject<ObserverAction.MemorySpawnerAction> subject { get; set; } = new(); 
    public List<(MemorySpawnObject,GameObject)> spawnedMemoryStageObject { get; set; } = new();
    public List<(SubMemorySpawnObject, GameObject, GameObject)> spawnedMemorySubStageObject { get; set; } = new();
    public List<int> spawnedMemoryID { get; set; } = new();


    private Dictionary<int, Dictionary<int, Dictionary<int, SubMemorySpawnObject>>> idToMemorySpawnSubGameobject = new();
    private Dictionary<int, Dictionary<int, MemorySpawnObject>> idToMemorySpawnGameobject = new();
    //default gameobject prefabs
    private GameObject subMemory3dPrefab;
    private GameObject subMemory2dPrefab;

    private readonly Vector3 subMemory3dOffset = new Vector3(250, 0 , 0); 


    [Header("add all clues here")]
    [SerializeField]
    private MemorySpawnScriptableObject[] memoryScriptableObjects;
    [SerializeField]
    private MemorySpawnerScriptableObject memorySpawnerScriptableObject;

    public override void m_Start()
    {

        subMemory2dPrefab = memorySpawnerScriptableObject.subMemory2dPrefab;
        subMemory3dPrefab = memorySpawnerScriptableObject.subMemory3dPrefab;
        base.m_Start();
        
    }
    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction(mm => { mm.memorySpawnerManager = this; });
        MemoryData.INSTANCE?.memoryTransformationHandler.subject.AddObserver(this);
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
                    single1.characterId = single.characterId; //setting memoryobject id to character memoryid
                    idToMemorySpawnGameobject[single.characterId].Add(single1.memoryId, single1);

                    idToMemorySpawnSubGameobject[single.characterId].Add(single1.memoryId, new());

                    foreach (var single2 in single1.subMemories)
                    {
                        idToMemorySpawnSubGameobject[single.characterId][single1.memoryId].Add(single2.id, single2);
                    }

                }
            }
    }

    //==Instantiate Memory==================================================================================
    public GameObject CreateMemoryStage(GameObject memoryStage)
    {
        return Instantiate(memoryStage, GameObject.FindGameObjectWithTag("MemoryParent").transform);
    }
    public GameObject CreateMemoryStage(GameObject memoryStage, GameObject stageParent) //USES LOCAL POSITION
    {
        GameObject g = Instantiate(memoryStage, stageParent.transform);
        return g;
    }
    //====================================================================================

    public void SpawnStageOnID(int characterID, int memoryID)
    {
        if (!idToMemorySpawnGameobject.ContainsKey(characterID))
        {
            Debug.LogError("character Id " + characterID +
            " not found for spawn stages.");

        }

        var memoryDict = idToMemorySpawnGameobject[characterID];

        MemorySpawnObject memoryStageObject = memoryDict[memoryID];

        int spawnPostionIndex = spawnedMemoryStageObject.Count;

        spawnedMemoryStageObject.Add((memoryStageObject,CreateMemoryStage(memoryStageObject.memoryGameObject)));

    }
    //TODO add this method with correct logic. 
    public void SpawnSubStageOnID(int characterID, int memoryID, int subMemoryId, GameObject parent)
    {
        SubMemorySpawnObject subMemory = idToMemorySpawnSubGameobject[characterID][memoryID][subMemoryId];

        //spawn the 2d submemory 
        GameObject subMemoryStageGameObject2D = CreateMemoryStage(subMemory2dPrefab, parent);
        subMemoryStageGameObject2D.GetComponent<SpriteRenderer>().sprite = subMemory.sprite;
        subMemoryStageGameObject2D.transform.localScale *= subMemory.scalar2d;

        //spawn the 3d submemory
        GameObject subMemoryStageGameObject3D = CreateMemoryStage(subMemory3dPrefab, parent);
        subMemoryStageGameObject3D.GetComponent<MeshFilter>().mesh = subMemory.mesh;
        subMemoryStageGameObject3D.transform.localScale *= subMemory.scalar3d;
        spawnedMemorySubStageObject.Add(new (subMemory,subMemoryStageGameObject2D, subMemoryStageGameObject3D));
        

        
       
    }
    //here we are 
    public void SetTransformStagePos()
    {

        foreach (var single in spawnedMemorySubStageObject)
        {
            //2d object
            PhysicsGameObject2d physicsGameObject2D = single.Item2.GetComponent<PhysicsGameObject2d>();
            //3d object
            TwoTo3dPositions twoTo3DPositions = single.Item3.GetComponent<TwoTo3dPositions>();


            twoTo3DPositions.physicsGameObject2D = physicsGameObject2D; //making sure 2d position aligns with 3d position. 
            Vector3 offset = physicsGameObject2D.transform.position + subMemory3dOffset;
            //change y position to zero
            offset = new Vector3(offset.x, 0, offset.z);
            
            twoTo3DPositions.SetPosition(offset);
            



        }
       
    }

    public void Load()
    {
        List<JsonUnlockMemoryObject> jsonUnlockMemoryObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonUnlockMemoryObject>(FileNames.UnlockedMemoryFile);


        jsonUnlockMemoryObjects.ForEach(character =>
        {
            foreach (var memoryObject in character.memoryObjects)
            {
                //spawning memory on load.
                SpawnStageOnID(character.characterID, memoryObject.ID);

                foreach (var subMemoryObject in memoryObject.subMemoryIds)
                {
                    //spawning sub memory on load.
                    var stageObj = spawnedMemoryStageObject[^1].Item2; //getting the memoryobject object.
                    SpawnSubStageOnID(character.characterID, memoryObject.ID, subMemoryObject.ID, stageObj);
                }
            }

            MemoryData.INSTANCE.spawnedMemoryStageObject = spawnedMemoryStageObject.ToArray();
            MemoryData.INSTANCE.currentCharacterID = character.characterID;
        });

        subject.NotifyObservers(ObserverAction.MemorySpawnerAction.onAfterStageObjectsSpawn);

    }

    public (FileNames, JsonObject[])[] Save()
    {
        throw new System.NotImplementedException();
    }

    public void OnNotify(MemoryTransform data)
    {
        if (data == MemoryTransform.onAfterTransformation)
        {
            SetTransformStagePos();
        }
    }
}