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
  private Dictionary<int, Dictionary<int, MemoryObject>> characterIdToMemory = new();
  private Dictionary<int,Dictionary<int, Dictionary<int, SubMemoryObject>>> characterMemoryToMemoryIdToSubMemory = new();

  private Dictionary<int,Dictionary<int, Dictionary<int, SubMemoryObject>>> UNLOCKEDCharacterMemoryToMemoryToSubMemory = new();
  private Dictionary<int, Dictionary<int, MemoryObject>> UNLOCKEDCharacterIdToMemory = new();

  public override void m_OnEnable()
  {
    MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) => { m.memoryManager = this; });
    base.m_OnEnable();
  }

  public void UnlockMemory(int characterID, int memoryID)
  {
    if (!characterIdToMemory.ContainsKey(characterID))
    {
      Debug.LogError("memory not found for character id " + characterID + "\n has the memory been added to character scriptable object? SEE character manager");
      return;
    }
    if (!characterIdToMemory[characterID].ContainsKey(memoryID))
    {
      Debug.LogError("memory not found for memory id " + memoryID + "in character " + characterID + "\nhas the memory been added to character scriptable object? SEE character manager");
      return;
    }
    //condition: if character exists in dictionary, has the memory already been unlocked?
    if (UNLOCKEDCharacterIdToMemory.ContainsKey(characterID))
    {
      if (UNLOCKEDCharacterIdToMemory[characterID].ContainsKey(memoryID))
      {
        Debug.LogError("memory has already been unlocked! for memory id " + memoryID + " in character " + characterID);
        return;
      }
    }

    MemoryObject m = characterIdToMemory[characterID][memoryID];

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
    if (!characterMemoryToMemoryIdToSubMemory.ContainsKey(memoryID))
    {
      Debug.LogError("submemory not found for unlocked memory id " + memoryID + "in character " + characterID + "has the memory been added to character scriptable object? SEE character manager");
      return;
    }

    //second, check if there is a sub memory in the memory
    if (!characterMemoryToMemoryIdToSubMemory[characterID].ContainsKey(memoryID))
    {
      Debug.LogError("SUBmemory not found for because unlocked memoryID " + memoryID + " doesn't exist");
      return;
    }
    if (!characterMemoryToMemoryIdToSubMemory[characterID][memoryID].ContainsKey(submemoryID))
    {
      Debug.LogError("SUBmemory not found because unlocked submemoryID" + submemoryID + " doesn't exist");
      return;
    }
    //is submemory already unlocked? 
    if (UNLOCKEDCharacterMemoryToMemoryToSubMemory.ContainsKey(characterID))
    {
      if (UNLOCKEDCharacterMemoryToMemoryToSubMemory[characterID].ContainsKey(memoryID))
      {
        if (UNLOCKEDCharacterMemoryToMemoryToSubMemory[characterID][memoryID].ContainsKey(submemoryID))
        {
          Debug.LogError("sub memory is already unlocked!");
          return;
        }
      } 
    }

    SubMemoryObject subM = characterMemoryToMemoryIdToSubMemory[characterID][memoryID][submemoryID];
    //finally, add sub memory to unlocked memory dict 
      if (!UNLOCKEDCharacterMemoryToMemoryToSubMemory.ContainsKey(characterID))
      {
        UNLOCKEDCharacterMemoryToMemoryToSubMemory.Add(characterID, new Dictionary<int, Dictionary<int, SubMemoryObject>>());
      }
      if (!UNLOCKEDCharacterMemoryToMemoryToSubMemory[characterID].ContainsKey(memoryID))
      {
        UNLOCKEDCharacterMemoryToMemoryToSubMemory[characterID].Add(memoryID, new Dictionary<int, SubMemoryObject>() { { submemoryID, subM } });
      }
      else
      {
        UNLOCKEDCharacterMemoryToMemoryToSubMemory[characterID][memoryID].Add(submemoryID, subM);
      }
    Debug.Log("LOG: UNLOCKED SUB MEMORY " + submemoryID);

  }
  public void AddMemory(int bodyID, MemoryObject memoryStage)
  {
    if (!characterIdToMemory.ContainsKey(bodyID))
    {
      characterIdToMemory.Add(bodyID, new Dictionary<int, MemoryObject>());
    }
    if(characterIdToMemory[bodyID].ContainsKey(memoryStage.ID))
    {
      Debug.LogError("memory " + memoryStage.ID + " already exist for character" + bodyID);
      return;
    }
    characterIdToMemory[bodyID].Add(memoryStage.ID, memoryStage);
    Debug.Log("LOG: added memory to memory dictionary. \n   You'll have to unlocked the memory first!");


    foreach (SubMemoryObject single in memoryStage.subMemoryIds)
    {
      AddSubMemory(bodyID,single.ID, single);
    }

  }
  public void AddSubMemory(int bodyId, int memoryID, SubMemoryObject subMemoryObject)
  {
    //duplicate sub memory? 
    if (characterMemoryToMemoryIdToSubMemory.ContainsKey(bodyId))
    {
      if (characterMemoryToMemoryIdToSubMemory[bodyId].ContainsKey(memoryID))
      {
        if (characterMemoryToMemoryIdToSubMemory[bodyId][memoryID].ContainsKey(subMemoryObject.ID))
        {
          Debug.LogError("sub memory is already added!");
          return;
        }
      }
    }


    if (!characterMemoryToMemoryIdToSubMemory.ContainsKey(bodyId))
    {
      characterMemoryToMemoryIdToSubMemory.Add(bodyId, new());
    }
    if (!characterMemoryToMemoryIdToSubMemory[bodyId].ContainsKey(memoryID))
      {
        characterMemoryToMemoryIdToSubMemory[bodyId].Add(memoryID, new Dictionary<int, SubMemoryObject>());
      }
    characterMemoryToMemoryIdToSubMemory[bodyId][memoryID].Add(subMemoryObject.ID, subMemoryObject);
    Debug.Log("LOG: added sub-memory to sub-memory dictionary. \nYou'll have to unlocked it first!");
  }


  public (FileNames, JsonObject[])[] Save()
  {
    //=memory files=======================================================================================================================
    JsonMemoryObject jsonMemoryObject = new(0, null);
    int[] characterkeys = characterIdToMemory.Keys.ToArray();

    foreach (int character in characterkeys)
    {
      Dictionary<int, MemoryObject> m = characterIdToMemory[character];
      int[] keys = m.Keys.ToArray();
      List<MemoryObject> memoryObjects1 = new();
      for (int i = 0; i < keys.Length; i++)
      {
        MemoryObject memoryObject = m[keys[i]];
        if (characterMemoryToMemoryIdToSubMemory.ContainsKey(memoryObject.ID))
        {
          Dictionary<int, SubMemoryObject> subMemoryObject = characterMemoryToMemoryIdToSubMemory[character][memoryObject.ID];
          SubMemoryObject[] sub = subMemoryObject.Values.ToArray();
          memoryObject.subMemoryIds = sub;
        }
        else
        {
          Debug.Log("no sub memories to be found");
        }
        memoryObjects1.Add(memoryObject);

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
        if (characterMemoryToMemoryIdToSubMemory.ContainsKey(memoryObject.ID))
        {
          Dictionary<int, SubMemoryObject> subMemoryObject = UNLOCKEDCharacterMemoryToMemoryToSubMemory[character][memoryObject.ID];
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
    List<JsonMemoryObject> jsonMemoryObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonMemoryObject>(FileNames.MemoryFile);
    jsonMemoryObjects.ForEach(characterMemory =>
    {

      foreach (var memory in characterMemory.memoryObject)
      {
        AddMemory(characterMemory.characterID, memory);
        foreach (var submemory in memory.subMemoryIds)
        {
          AddSubMemory(characterMemory.characterID, memory.ID, submemory);
        }
      }

    });
    List<JsonUnlockMemoryObject> jsonUnlockMemoryObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonUnlockMemoryObject>(FileNames.UnlockedMemoryFile);
    jsonUnlockMemoryObjects.ForEach(characterMemory =>
    {
      foreach (var memory in characterMemory.memoryObjects)
      {
        UnlockMemory(characterMemory.characterID, memory.ID);
        foreach (var submemory in memory.subMemoryIds)
        {
          UnlockSubMemory(characterMemory.characterID, memory.ID, submemory.ID);
        }
      }
    });
    
  }
}
