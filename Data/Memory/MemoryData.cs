using System.Collections;
using System.Collections.Generic;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

public class MemoryData : StaticInstance<MemoryData>
{
    public List<MemoryStage> memoryStages {get; set;} = new List<MemoryStage>();

    public override void OnEnable()
    {
       
    }


}
