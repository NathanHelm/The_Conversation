using System.Collections;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

public class MemoryData : StaticInstanceData<MemoryData>
{
    public MemoryStage[] memoryStages {get; set;}  //all memories in the dream scene.


    public override void OnEnable()
    {
     //todo memoryStages = PersistentFile.getCurrentStages
    }


}
