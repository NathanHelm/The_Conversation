using UnityEngine;
using System.Collections;
using Data;
public class PlayerMove2dState : PlayerState
{
    public override void OnEnter(PlayerData data)
    {
        Debug.Log("player move 2d state");
        
    }
    public override void OnFixedUpdate(PlayerData data)
    {
        data?.playerMovement.PlayerMovementFunction();
    }
    public override void OnExit(PlayerData data)
    {
        data?.playerMovement.SetPlayer3dRotation();
        data?.playerMovement.stopMovement();
    }
}

