using System.Collections.Generic;
using ObserverAction;
using UnityEngine;
using Data;
using System.Linq;
using MemorySpawn;
using System;


public class MemoryTransformationHandler : MonoBehaviour, IObserver<ObserverAction.MemorySpawnerAction>, IExecution
{
    public SystemActionCall<MemoryTransformationHandler> onEnableTransformations = new();
    public SystemActionCall<MemoryTransformationHandler> onMemoryTransformGameExecute = new();
    public List<MemorySpawnObject> spawnedStages { get; set; } = new(); //get from memorydata on start.
    public int currentCharacterID { get; set; }

    //========================================================================================================================================================================

    private List<MemorySpawnObject> randomUpdateMovementStages = new(); //"randomly picked" objects that will undergo random transforms on update
    private List<ObserverAction.MemoryTransformUpdateAction> randomUpdateStageActions = new(); //"randomly picked" update actions that will be added to associated spawnUpdateMovement.

    //========================================================================================================================================================================

    public Subject<ObserverAction.MemoryTransformEnableAction> subjectEnable { get; set; } = new();
    public SubjectActionData<ObserverAction.MemoryTransformUpdateAction, MemorySpawnObject> subjectUpdate { get; set; } = new();
    Dictionary<int,Dictionary<int, ObserverAction.MemoryTransformEnableAction>> memoryTransformOnEnable = new();
    Dictionary<int,Dictionary<int, ObserverAction.MemoryTransformUpdateAction>>  memoryTransformOnUpdate = new();

    private readonly int uniqueStageMovementAmount = 6;


    private void PopulateTransformDictionary()
    {
        //for on enable
        memoryTransformOnEnable = new Dictionary<int, Dictionary<int, MemoryTransformEnableAction>>()
        {
           {
                42, new Dictionary<int, MemoryTransformEnableAction>()
                {
                  { 100, MemoryTransformEnableAction.circleEnable},
                }
            },
        };
        //for update
        memoryTransformOnUpdate = new Dictionary<int, Dictionary<int, MemoryTransformUpdateAction>>()
        {
            {
                42, new Dictionary<int, MemoryTransformUpdateAction>()
                {
                    { 100, MemoryTransformUpdateAction.circleUpdate },
                }
            },
        };
    }


    public void MemoriesTransformOnEnable()
    {
        //based on the memory ID that is being used, alter the movement
        int currentMemoryID = MemoryData.INSTANCE.currentMemoryID;
        MemorySpawnObject currentMemory = spawnedStages[0];

        randomUpdateMovementStages = GetStageMovementAtRandomUpdate();
        randomUpdateStageActions = GetRandomUpdateAction(randomUpdateMovementStages.Count);

        if (!memoryTransformOnEnable.ContainsKey(currentMemoryID))
        {
            SetStageMovementAtRandomEnable(); //if there is no unique movment that is required for the memory id, we can just randomly assign some movement to the memory objects
        }
        else
        {
            //based on current memory id, get on enable tranformation
            ObserverAction.MemoryTransformEnableAction memorySpawnerAction = memoryTransformOnEnable[currentMemory.characterId][currentMemoryID];

            subjectEnable.NotifyObservers(memorySpawnerAction);
        }
    }
    public void MemoriesTransformOnUpdate()
    {
        SetRandomMemoryStageToUpdateAction(); //run observers
    }
   
    private void SetRandomMemoryStageToUpdateAction() //(0)
    {
        //run on update
        for (int i = 0; i < randomUpdateMovementStages.Count; i++)
        {
            subjectUpdate.NotifyObservers(randomUpdateStageActions[i], randomUpdateMovementStages[i]);
        }
    }


    private List<MemorySpawnObject> GetStageMovementAtRandomUpdate() //(1) get random stage
    {
        int i = uniqueStageMovementAmount;
        List<MemorySpawnObject> temp = spawnedStages.GetRange(0,spawnedStages.Count);
        List<MemorySpawnObject> stagesUpdateTransformation = new(); //our object that will undergo update transformations. 
        while (i > 0 && temp.Count > 0)
        {
            //5 - 5 = 0 < count  run
            //5 - 4 = 1 = count dont run
            /*
            if uniqueStage > stages 
            if (uniqueStageMovementAmount - i > temp.Count)
            {
                Debug.Log("breaking because " + i + " is > than temp count " + temp.Count);
                break;
            }
            */
            int random = UnityEngine.Random.Range(0, temp.Count - 1);
            stagesUpdateTransformation.Add(temp[random]);
            temp.RemoveAt(random);
            --i;
        }
        return stagesUpdateTransformation;

    }
     private List<MemoryTransformUpdateAction> GetRandomUpdateAction(int maxSpawnStage) //(2) get random action for randomly selected stage.
    {
        int currentCharacterID = spawnedStages[0].characterId;

        List<ObserverAction.MemoryTransformUpdateAction> randomUpdateStageActionsTemp = new(); //randomly update observer actions

        ObserverAction.MemoryTransformUpdateAction[] allMemoryTransformUpdateActions =
        memoryTransformOnUpdate[currentCharacterID].Values.ToArray(); 


       
        for (int i = 0; i < maxSpawnStage; i++)
        {

            int memoryID = randomUpdateMovementStages[i].memoryId;
        
            if (memoryTransformOnUpdate[currentCharacterID].ContainsKey(memoryID)) //if memory id already has an action, use it
            {
                var observerAction = memoryTransformOnUpdate[currentCharacterID][memoryID];
                randomUpdateStageActionsTemp.Add(observerAction);
            }
            else
            {
                int random = UnityEngine.Random.Range(0, allMemoryTransformUpdateActions.Length);
                randomUpdateStageActionsTemp.Add(allMemoryTransformUpdateActions[random]);
            }
        }
        

        return randomUpdateStageActionsTemp;
    }
    
    private void SetStageMovementAtRandomEnable()
    {
        MemoryTransformEnableAction[] onEnableTranformEnums = memoryTransformOnEnable[currentCharacterID].Values.ToArray();
        int random = UnityEngine.Random.Range(0, onEnableTranformEnums.Length - 1);
        subjectEnable.NotifyObservers(onEnableTranformEnums[random]); //will play a random onenable observer action.
    }

    public void OnNotify(MemorySpawnerAction data)
    {
        if (data == MemorySpawnerAction.onAfterStageObjectsSpawn) //here we are running the movement code
        {
            onEnableTransformations.RunAction(this);
            PopulateTransformDictionary();
            MemoriesTransformOnEnable();
        }

        if (data == MemorySpawnerAction.onTransformObjectUpdate)
        {
            MemoriesTransformOnUpdate();
        }
       
    }
    public void m_Awake()
    {

    }
    public void m_OnEnable()
    {
        MemorySpawnerManager.INSTANCE.subject.AddObserver(this); //add observer to spawner
    }
    public void m_GameExecute()
    {
        onMemoryTransformGameExecute.RunAction(this);
       
    }
}