using Codice.ThemeImages;
using UnityEditor;
using UnityEngine;

namespace MemorySpawn
{

   [System.Serializable]
   public class MemorySpawnObject
   {
      
      public int memoryId;
      public int characterId; 
      public GameObject memoryGameObject;
      public Vector3 memoryPos;
      public SubMemorySpawnObject[] subMemories;
      [Header("offset postion from on enable transformation")]
      public Vector3 enableOffsetPostion = new();
     
      public MemorySpawnObject(Vector3 enableOffset,int characterId, int memoryId, GameObject memoryPrefab, Vector3 memoryPos, SubMemorySpawnObject[] subMemories)
      {
         this.enableOffsetPostion = enableOffset;
         this.characterId = characterId;
         this.memoryId = memoryId;
         this.memoryGameObject = memoryPrefab;
         this.memoryPos = memoryPos;
         this.subMemories = subMemories;
      }
    }

}