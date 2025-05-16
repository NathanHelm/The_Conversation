using System.Diagnostics;
using Data;

public class IdleSceneState : SceneState{
    public override void OnEnter(SceneData data)
    {
        UnityEngine.Debug.Log("in idle scene state");
    }
    
}