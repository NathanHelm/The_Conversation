using UnityEngine;

public class SubMemoryStageCreatedObject
{
    public SubMemoryStageCreatedObject(GameObject subStage2d, GameObject subStage3d)
    {
        this.subStage2d = subStage2d;
        this.subStage3d = subStage3d;
    }

    public GameObject subStage2d { get; set; }
    public GameObject subStage3d { get; set; }
    
}