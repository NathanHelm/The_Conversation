using System;
using System.Collections.Generic;
using MemorySpawn;
using ObserverAction;
using UnityEngine;
 
public class MemoryTransformCircle : MemoryTransformations
{

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
   
    

    public override void TransformOnEnable(ref (MemorySpawnObject,GameObject)[] spawnedMemoryObjects)
    {
        Debug.Log("circle transformation on enable");
        for (int i = 0; i < spawnedMemoryObjects.Length; i++)
        {
            Transform transform = spawnedMemoryObjects[i].Item2.transform;

            angleEnable += freqEnable * Mathf.Deg2Rad;
            randomValues.Add(UnityEngine.Random.Range(1, 2));

            transform.position = new Vector3(Mathf.Cos(angleEnable), Mathf.Sin(angleEnable)) * ampEnable;

            spawnedMemoryObjects[i].Item1.enableOffsetPostion = transform.position;
           
        }
    }

    public override void TransformOnUpdate(ref MemorySpawnObject spawnedMemoryObject)
    {
        Debug.Log("circle transformation on update");
       
        Transform trans = spawnedMemoryObject.memoryGameObject.transform;
        float random =  randomValues[spawnedMemoryObject.positionIndex];
        angle += Time.deltaTime * freq * random;
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
    public override void OnNotify(MemoryTransformUpdateAction data, MemorySpawnObject memoryObject)
    {
        if (data == MemoryTransformUpdateAction.circleUpdate)
        {
            TransformOnUpdate(ref memoryObject);
        }
       // base.OnNotify(data);
    }
}