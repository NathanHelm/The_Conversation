using UnityEngine;
using System.Collections;
using Data;
public class PlayerMove2dState : State<PlayerData>
{
    public override void OnEnter(PlayerData data)
    {
     
    }
    public override void OnFixedUpdate(PlayerData data)
    {
        data.playerMovement.PlayerMovementFunction();
    }
    public override void OnExit(PlayerData data)
    {
        data.playerMovement.coroutine = null;
    }
}

