using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : StaticInstance<MemoryManager>
{
    public Dictionary<int, MemoryStage> memoryIdtoMemoryStagesDictionary {get; set;} = new Dictionary<int, MemoryStage>();

    public override void OnEnable()
    {
       MManager.INSTANCE.actionCall.AddAction((MManager m)=>{m.memoryManager = this;});
       base.OnEnable();
    }
    public override void m_Start()
    {
       
    }
    public void AddCharacterMemoryToSpawner(int characterID, int memoryID)
    {
        //adds the memory to the spawner class, to instantiate memories in the 'dream' scene.

        Debug.Log("added memory "+memoryID+" to spawner");


    }
    public void RemoveMemoryToSpawner(int memoryID)
    {

    }
    public void Spawn()
    {
        //todo make this work for persitent data.
        
       // if(Mem)
      //  memoryStages.
      
    } 
  //  public 
}
