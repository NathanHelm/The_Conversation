using Codice.ThemeImages;
using UnityEditor;
using UnityEngine;

namespace MemorySpawn
{

   [System.Serializable]
   public class MemorySpawnObject
   {
      
      public int memoryId;
      [Header("no need to input id here.")]
      public int characterId; 
      public GameObject memoryGameObject;
      public Vector3 memoryPos;
      public SubMemorySpawnObject[] subMemories;
      [Header("offset postion from on enable transformation")]
      public Vector3 enableOffsetPostion = new();
      public int positionIndex = -1;
     
      public MemorySpawnObject(int positionIndex,Vector3 enableOffset, int characterId, int memoryId, GameObject memoryPrefab, Vector3 memoryPos, SubMemorySpawnObject[] subMemories)
      {
         this.positionIndex = positionIndex;
         this.enableOffsetPostion = enableOffset;
         this.characterId = characterId;
         this.memoryId = memoryId;
         this.memoryGameObject = memoryPrefab;
         this.memoryPos = memoryPos;
         this.subMemories = subMemories;
      }
    }

}