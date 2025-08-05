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
    public List<(MemorySpawnObject,GameObject)> spawnedStages { get; set; } = new(); //get from memorydata on start.
    public int currentCharacterID { get; set; }

    public Subject<ObserverAction.MemoryTransform> subject { get; set;} = new();

    //========================================================================================================================================================================

    private List<MemorySpawnObject> randomUpdateMovementStages = new(); //"randomly picked" objects that will undergo random transforms on update
    private List<ObserverAction.MemoryTransformUpdateAction> randomUpdateStageActions = new(); //"randomly picked" update actions that will be added to associated spawnUpdateMovement.

    //========================================================================================================================================================================

    public Subject<ObserverAction.MemoryTransformEnableAction> subjectEnable { get; set; } = new();
    public SubjectActionData<ObserverAction.MemoryTransformUpdateAction, MemorySpawnObject> subjectUpdate { get; set; } = new();
    Dictionary<int,Dictionary<int, ObserverAction.MemoryTransformEnableAction>> memoryTransformOnEnable = new();
    Dictionary<int,Dictionary<int, ObserverAction.MemoryTransformUpdateAction>>  memoryTransformOnUpdate = new();

    private readonly int uniqueStageMovementAmount = 6;

    /*
    memory transform handler-- providing memory gameobject movement based on unlocked memory ids. 
    
    PopulateTransformDictionary: adds transforms (both enable and update) to specific memory id. 
    
    */

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
        //dummy
        memoryTransformOnEnable = new()
        {
            {
                100, new()
                {
                    {
                        100, MemoryTransformEnableAction.circleEnable
                    },
                }
            },
        };
        memoryTransformOnUpdate = new()
        {
            {
                100, new()
                {
                    {
                        100, MemoryTransformUpdateAction.circleUpdate
                    },
                }
            },
        };
        
        
    }


    public void MemoriesTransformOnEnable()
    {
        //based on the memory ID that is being used, alter the movement
        int currentMemoryID = MemoryData.INSTANCE.recentUnlockedMemoryId;
        MemorySpawnObject currentMemory = spawnedStages[0].Item1;

        randomUpdateMovementStages = GetStageMovementAtRandomUpdate();

        //the random transformation that will run on update.
        randomUpdateStageActions = GetRandomUpdateAction(randomUpdateMovementStages.Count);

        if (!memoryTransformOnEnable.ContainsKey(currentMemoryID))
        {
            SetStageMovementAtRandomEnable(); //if there is no unique movment that is required for the memory id, we can just randomly assign some movement to the memory objects
        }
        else
        {
            //based on current memory id, get on enable tranformation
            ObserverAction.MemoryTransformEnableAction memorySpawnerAction = memoryTransformOnEnable[currentMemory.characterId][currentMemoryID];

            subjectEnable.NotifyObservers(memorySpawnerAction); //run on enable transformation(s).
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
        List<(MemorySpawnObject,GameObject)> temp = spawnedStages.GetRange(0,spawnedStages.Count);
        List<MemorySpawnObject> stagesUpdateTransformation = new(); //our object that will undergo update transformations. 
        while (i > 0 && temp.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, temp.Count - 1);
            stagesUpdateTransformation.Add(temp[random].Item1);
            temp.RemoveAt(random);
            --i;
        }
        return stagesUpdateTransformation;

    }
     private List<MemoryTransformUpdateAction> GetRandomUpdateAction(int maxSpawnStage) //(2) get random action from randomly selected stage.
    {
        int currentCharacterID = spawnedStages[0].Item1.characterId;

        List<ObserverAction.MemoryTransformUpdateAction> randomUpdateStageActionsTemp = new(); //randomly update observer actions

        ObserverAction.MemoryTransformUpdateAction[] allMemoryTransformUpdateActions = new MemoryTransformUpdateAction[0];

        if (!memoryTransformOnUpdate.ContainsKey(currentCharacterID))
        {
            Debug.LogError("it does appear that characterID " + currentCharacterID + " is found, therefore we will be using character ID 100");
            allMemoryTransformUpdateActions =
            memoryTransformOnUpdate[100].Values.ToArray();
        }
        else
        {

            allMemoryTransformUpdateActions =
            memoryTransformOnUpdate[currentCharacterID].Values.ToArray();
        }


       
        for (int i = 0; i < maxSpawnStage; i++)
            {

                int memoryID = randomUpdateMovementStages[i].memoryId;

            if (!memoryTransformOnUpdate.ContainsKey(currentCharacterID))
            {
                int random = UnityEngine.Random.Range(0, allMemoryTransformUpdateActions.Length);
                randomUpdateStageActionsTemp.Add(allMemoryTransformUpdateActions[random]);
            }
            else if (!memoryTransformOnUpdate[currentCharacterID].ContainsKey(memoryID)) //if memory id already has an action, use it
            {
                int random = UnityEngine.Random.Range(0, allMemoryTransformUpdateActions.Length);
                randomUpdateStageActionsTemp.Add(allMemoryTransformUpdateActions[random]);
            }
            else
            {
                var observerAction = memoryTransformOnUpdate[currentCharacterID][memoryID];
                randomUpdateStageActionsTemp.Add(observerAction);
            }
            }
        

        return randomUpdateStageActionsTemp;
    }
    
    private void SetStageMovementAtRandomEnable()
    {
        int checkCurrentCharacterID = currentCharacterID;
        if (!memoryTransformOnEnable.ContainsKey(checkCurrentCharacterID))
        {
            checkCurrentCharacterID = 100;
        }
        MemoryTransformEnableAction[] onEnableTranformEnums = memoryTransformOnEnable[checkCurrentCharacterID].Values.ToArray();
        int random = UnityEngine.Random.Range(0, onEnableTranformEnums.Length - 1);
        subjectEnable.NotifyObservers(onEnableTranformEnums[random]); //will play a random onenable observer action.
    }

    //run this after onEnable transformation!
    public void MoveNoUpdateTransToNewParent(ref GameObject memoryStage)
    {
        //This solves the issue of have a stage that moves, but the clues/objects that don't
        int i = 0;
        while (i < memoryStage.transform.childCount)
        {
            Transform child = memoryStage.transform.GetChild(i);

            if (child.CompareTag("NoUpdateTrans"))
            {
                child.SetParent(GameObject.FindWithTag("NoUpdateParent").transform);
                // Do NOT increment i here, because the next child has shifted into index i
            }
            else
            {
                i++; // Only move to the next child if current one wasn't removed
            }
        }

    }

    public void OnNotify(MemorySpawnerAction data)
    {
        if (data == MemorySpawnerAction.onAfterStageObjectsSpawn) //here we are running the movement code
        {
            onEnableTransformations.RunAction(this);
            PopulateTransformDictionary();
            MemoriesTransformOnEnable();

            foreach (var single in spawnedStages) //change children to new object to avoid update transformation.
            {
                var memoryStage = single.Item2;
                MoveNoUpdateTransToNewParent(ref memoryStage);

            }
            subject.NotifyObservers(MemoryTransform.onAfterTransformation);

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