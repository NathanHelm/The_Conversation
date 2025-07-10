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
   
    

    public override void TransformOnEnable(ref MemorySpawnObject[] spawnedMemoryObjects)
    {
        Debug.Log("circle transformation on enable");
        for (int i = 0; i < spawnedMemoryObjects.Length; i++)
        {
            Transform transform = spawnedMemoryObjects[i].memoryGameObject.transform;

            angleEnable += Time.deltaTime * freqEnable;

            transform.position = new Vector3(Mathf.Cos(angleEnable), Mathf.Sin(angleEnable)) * ampEnable;

            spawnedMemoryObjects[i].enableOffsetPostion = transform.position;
        }
    }

    public override void TransformOnUpdate(ref MemorySpawnObject spawnedMemoryObject)
    {
        Debug.Log("circle transformation on update");
        Transform trans = spawnedMemoryObject.memoryGameObject.transform;
        angle += Time.deltaTime * freq;
        trans.position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * amp + spawnedMemoryObject.enableOffsetPostion;
    }
    public override void OnNotify(MemoryTransformEnableAction data)
    {
        if (data == MemoryTransformEnableAction.circleEnable)
        {
            spawnedMemoryObjects = Data.MemoryData.INSTANCE.memorySpawnObjects;
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