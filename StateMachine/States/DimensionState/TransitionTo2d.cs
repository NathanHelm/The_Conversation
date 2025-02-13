using UnityEngine;
using System.Collections;
using Data;
public class TransitionTo2d : DimensionState
{
    public override void OnEnter(DimensionData data)
    {
        Debug.Log("2d state");
        data?.ToDimension(data.TransitionTo2dSo);
    }
    public override void OnExit(DimensionData data)
    {
        if (data != null)
        {
            data.TransitionTo3dSo.cinemachineVirtualCamera.Priority = 0;
        }
    }
}

