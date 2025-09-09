using MemorySpawn;
using UnityEngine;

public interface IMemoryTransforms
{
    public void TransformOnUpdate(ref MemoryStageCreatedObject memoryObjects);
    public void TransformOnEnable(ref MemoryStageCreatedObject[] memoryObject);
}