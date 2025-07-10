using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Persistence;
using UnityEngine;

public class MemoryManager : StaticInstance<MemoryManager>, ISaveLoad, IExecution
{
  //characterID---memoryID---submemoryID
  private Dictionary<int, Dictionary<int, MemoryObject>> characterIDToMemory = new();
  private Dictionary<int, Dictionary<int, SubMemoryObject>> memoryIdToSubMemory = new();

  private Dictionary<int, Dictionary<int, SubMemoryObject>> UNLOCKEDMemoryIdToSubMemory = new();
  private Dictionary<int, Dictionary<int, MemoryObject>> UNLOCKEDCharacterIdToMemory = new();

  public override void m_OnEnable()
  {
    MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) => { m.memoryManager = this; });
    base.m_OnEnable();
  }

  public void UnlockMemory(int characterID, int memoryID)
  {
    if (!characterIDToMemory.ContainsKey(characterID))
    {
      Debug.LogError("memory not found for character id" + characterID);
      return;
    }
    if (!characterIDToMemory[characterID].ContainsKey(memoryID))
    {
      Debug.LogError("memory not found for memory id" + memoryID + "in character " + characterID);
      return;
    }
    MemoryObject m = characterIDToMemory[characterID][memoryID];

    if (!UNLOCKEDCharacterIdToMemory.ContainsKey(characterID))
    {
      UNLOCKEDCharacterIdToMemory.Add(characterID, new Dictionary<int, MemoryObject>() { { memoryID, m } });
    }
    else
    {
      UNLOCKEDCharacterIdToMemory[characterID].Add(memoryID, m);
    }
    Debug.Log("LOG: UNLOCKED MEMORY " + memoryID);

  }
  public void UnlockSubMemory(int characterID, int memoryID, int submemoryID)
  {
    //first, check if there is a character
    if (!UNLOCKEDCharacterIdToMemory.ContainsKey(characterID))
    {
      Debug.LogError("submemory not found for unlocked character id" + characterID);
      return;
    }
    if (!UNLOCKEDCharacterIdToMemory[characterID].ContainsKey(memoryID))
    {
      Debug.LogError("submemory not found for unlocked memory id" + memoryID + "in character " + characterID);
      return;
    }

    //second, check if there is a sub memory in the memory
    if (!memoryIdToSubMemory.ContainsKey(memoryID))
    {
      Debug.LogError("SUBmemory not found for because unlocked memoryID " + memoryID + " doesn't exist");
      return;
    }
    if (!memoryIdToSubMemory[memoryID].ContainsKey(submemoryID))
    {
      Debug.LogError("SUBmemory not found because unlocked submemoryID" + submemoryID + " doesn't exist");
      return;
    }

    SubMemoryObject subM = memoryIdToSubMemory[memoryID][submemoryID];
    //finally, add sub memory to unlocked memory dict 
    if (!UNLOCKEDMemoryIdToSubMemory.ContainsKey(memoryID))
    {
      UNLOCKEDMemoryIdToSubMemory.Add(memoryID, new Dictionary<int, SubMemoryObject>() { { submemoryID, subM } });
    }
    else
    {
      UNLOCKEDMemoryIdToSubMemory[memoryID].Add(submemoryID, subM);
    }
    Debug.Log("LOG: UNLOCKED SUB MEMORY " + submemoryID);

  }
  public void AddMemory(int bodyID, MemoryObject memoryStage)
  {
    if (!characterIDToMemory.ContainsKey(bodyID))
    {
      characterIDToMemory.Add(bodyID, new Dictionary<int, MemoryObject>());
    }
    characterIDToMemory[bodyID].Add(memoryStage.ID, memoryStage);
    Debug.Log("LOG: added memory to memory dictionary. \n   You'll have to unlocked the memory first!");


    foreach (SubMemoryObject single in memoryStage.subMemoryIds)
    {
      AddSubMemory(single.ID, single);
    }

  }
  public void AddSubMemory(int memoryID, SubMemoryObject subMemoryObject)
  {
    if (!memoryIdToSubMemory.ContainsKey(memoryID))
    {
      memoryIdToSubMemory.Add(memoryID, new Dictionary<int, SubMemoryObject>());
    }
    memoryIdToSubMemory[memoryID].Add(subMemoryObject.ID, subMemoryObject);
    Debug.Log("LOG: added sub-memory to sub-memory dictionary. \nYou'll have to unlocked it first!");
  }


  public (FileNames, JsonObject[])[] Save()
  {
    //=memory files=======================================================================================================================
    JsonMemoryObject jsonMemoryObject = new(0, null);
    int[] characterkeys = characterIDToMemory.Keys.ToArray();

    foreach (int character in characterkeys)
    {
      Dictionary<int, MemoryObject> m = UNLOCKEDCharacterIdToMemory[character];
      int[] keys = m.Keys.ToArray();
      List<MemoryObject> memoryObjects1 = new();
      for (int i = 0; i < keys.Length; i++)
      {
        MemoryObject memoryObject = m[keys[i]];
        if (memoryIdToSubMemory.ContainsKey(memoryObject.ID))
        {
          Dictionary<int, SubMemoryObject> subMemoryObject = memoryIdToSubMemory[memoryObject.ID];
          SubMemoryObject[] sub = subMemoryObject.Values.ToArray();
          memoryObject.subMemoryIds = sub;
        }
        else
        {
          Debug.Log("no sub memories to be found");
        }


      }
       jsonMemoryObject = new JsonMemoryObject(character, memoryObjects1.ToArray());

    }
  
  //=unlocked memory files=======================================================================================================================
  JsonUnlockMemoryObject unlockedJsonMemoryObject = new(0, null);

  int[] characterkeys1 = UNLOCKEDCharacterIdToMemory.Keys.ToArray();

    foreach (int character in characterkeys1)
    {
      Dictionary<int, MemoryObject> unlockedM = UNLOCKEDCharacterIdToMemory[character];
      int[] keys = unlockedM.Keys.ToArray();
      List<MemoryObject> memoryObjects = new();
      for (int i = 0; i < keys.Length; i++)
      {
        MemoryObject memoryObject = unlockedM[keys[i]];
        if (memoryIdToSubMemory.ContainsKey(memoryObject.ID))
        {
          Dictionary<int, SubMemoryObject> subMemoryObject = UNLOCKEDMemoryIdToSubMemory[memoryObject.ID];
          var sub = subMemoryObject.Values.ToArray();
          memoryObject.subMemoryIds = sub;
        }
        else
        {
          Debug.Log("no sub memories to be found");
        }
        memoryObjects.Add(memoryObject);

      }
       
      unlockedJsonMemoryObject = new JsonUnlockMemoryObject(character, memoryObjects.ToArray());

      }
    return new (FileNames, JsonObject[])[]{
      new(FileNames.MemoryFile, new JsonMemoryObject[] { jsonMemoryObject }),
      new(FileNames.UnlockedMemoryFile, new JsonUnlockMemoryObject[] { unlockedJsonMemoryObject })
    };
  }

    public void Load()
  {
    throw new System.NotImplementedException();
  }
}
