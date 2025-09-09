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

     
      public MemorySpawnObject(int characterId, int memoryId, GameObject memoryPrefab, Vector3 memoryPos, SubMemorySpawnObject[] subMemories)
      {
         this.characterId = characterId;
         this.memoryId = memoryId;
         this.memoryGameObject = memoryPrefab;
         this.memoryPos = memoryPos;
         this.subMemories = subMemories;
      }
    }

}