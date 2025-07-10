using System.Collections.Generic;
using System.Linq;
using MemorySpawn;
using ObserverAction;
using UnityEngine;

public class MemoryTransformations : MonoBehaviour, IMemoryTransforms, IExecution, IObserver<ObserverAction.MemoryTransformEnableAction>, IObserverData<ObserverAction.MemoryTransformUpdateAction, MemorySpawnObject>
{
    protected MemorySpawnObject[] spawnedMemoryObjects;

    public virtual void m_Awake()
    {
    }

    public virtual void m_OnEnable()
    {

        MemoryTransformationHandler memoryTransformationHandler = Data.MemoryData.INSTANCE.memoryTransformationHandler;
        memoryTransformationHandler.subjectEnable.AddObserver(this);
        memoryTransformationHandler.subjectUpdate.AddObserver(this);

    }
    public virtual void m_GameExecute()
    {
    }
    public virtual void TransformOnUpdate(ref MemorySpawnObject memoryObjects)
    {
    }

    public virtual void TransformOnEnable(ref MemorySpawnObject[] memoryObject)
    {
    }


    public virtual void OnNotify(MemoryTransformEnableAction data)
    {

    }

    public virtual void OnNotify(MemoryTransformUpdateAction data, MemorySpawnObject memoryObject)
    {
    
    }
}
