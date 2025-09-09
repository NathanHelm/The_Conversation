using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using MemorySpawn;
using ObserverAction;
using UnityEngine;
 
public class MemoryTransformCircle : MemoryTransformations
{
    [Header("options")]
    [Header("\n number determines how many random amplitudes\n there will be for all spawned memory stages")]
    [SerializeField]
    private float amountOfRandomnessForStages = 4;

    [Header("on update")]
    [SerializeField]
    private float amp = 1;
     [SerializeField]
    private float freq = 1;
    float angle = 0;
    //====================================
    [Header("on enable")]
    [SerializeField]
    private float ampEnable = 1;
     [SerializeField]
    private float freqEnable = 1;
    float angleEnable = 0;

    private Vector3 enableOffset;
    private List<float> randomValues = new();
   
    

    public override void TransformOnEnable(ref MemoryStageCreatedObject[] memoryStageCreatedObjects)
    {
        Debug.Log("circle transformation on enable");
        foreach (var memoryStageCreatedObject in memoryStageCreatedObjects)
        {
            var spawnedStage = memoryStageCreatedObject.memorySpawnObject;

            Transform trans = spawnedStage.memoryGameObject.transform;
            angleEnable += freqEnable * Mathf.Deg2Rad;
            trans.position = new Vector3(Mathf.Cos(angleEnable), Mathf.Sin(angleEnable)) * ampEnable;
            memoryStageCreatedObject.enableOffsetPostion = trans.position;
            foreach (var submemory2d in memoryStageCreatedObject.spawnedSubMemory)
            {
                submemory2d.subStage2d.transform.position = trans.position;
            }
        }
    }

    public override void TransformOnUpdate(ref MemoryStageCreatedObject spawnedMemoryObject)
    {
        Debug.Log("circle transformation on update");

        
            Transform trans = spawnedMemoryObject.spawnedMemoryStage.transform;
            // float random = randomValues[spawnedMemoryObject.positionIndex];
            float random = spawnedMemoryObject.memorySpawnObject.memoryId % amountOfRandomnessForStages;
            angle += Time.deltaTime * freq * Mathf.InverseLerp(0,amountOfRandomnessForStages,random);
            trans.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * amp + spawnedMemoryObject.enableOffsetPostion;
        
    }
    public override void OnNotify(MemoryTransformEnableAction data)
    {
        if (data == MemoryTransformEnableAction.circleEnable)
        {
            var spawnedMemoryObjects = Data.MemoryData.INSTANCE.spawnedMemoryStageObject;
           
            TransformOnEnable(ref spawnedMemoryObjects);
        }
    }
    public override void OnNotify(MemoryTransformUpdateAction data, MemoryStageCreatedObject memoryStageCreatedObject)
    {
        if (data == MemoryTransformUpdateAction.circleUpdate)
        {
            TransformOnUpdate(ref memoryStageCreatedObject);
        }
       // base.OnNotify(data);
    }
}