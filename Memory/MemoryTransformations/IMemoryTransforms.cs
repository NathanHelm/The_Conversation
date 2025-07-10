using MemorySpawn;
using UnityEngine;

public interface IMemoryTransforms
{
    public void TransformOnUpdate(ref MemorySpawnObject memoryObjects);
    public void TransformOnEnable(ref MemorySpawnObject[] memoryObject);
}