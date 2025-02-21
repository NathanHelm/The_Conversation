using UnityEngine;
using System.Collections;
using Data;
public class TransitionTo2d : DimensionState
{
    public override void OnEnter(DimensionData data)
    {
        Debug.Log("2d state");
        TransitionManager.INSTANCE.ToDimension(data.TransitionTo2dSo);
    }
    public override void OnExit(DimensionData data)
    {
        if (data != null)
        {
            TransitionManager.INSTANCE.SetCamToZero(data.TransitionTo2dSo);
        }
    }
}

