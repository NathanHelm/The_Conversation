using UnityEngine;
using System.Collections;
using Data;
public class TransitionTo2d : State<DimensionData>
{
    public override void OnEnter(DimensionData data)
    {
        
        data.ToDimension(data.TransitionTo2dSo);
    }
    public override void OnExit(DimensionData data)
    {
        data.TransitionTo3dSo.cinemachineVirtualCamera.Priority = 0; 
    }
}

