using UnityEngine;
using System.Collections;
using Data;
public class TransitionTo3d : DimensionState
{
    public override void OnEnter(DimensionData data)
    {
        Debug.Log("3d state");
        data.ToDimension(data.TransitionTo3dSo);
    }
    public override void OnExit(DimensionData data)
    {
        data.TransitionTo3dSo.cinemachineVirtualCamera.Priority = 0;
    }
    public override void OnUpdate(DimensionData data)
    {
      data.TransitionTo3dSo.cinemachineVirtualCamera.transform.rotation  =  PlayerData.INSTANCE.trans3d.rotation;
    }
}

