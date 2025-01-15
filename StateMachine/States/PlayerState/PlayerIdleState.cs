using UnityEngine;
using System.Collections;
using Data;
public class PlayerIdleState : State<PlayerData>
{
    public override void OnEnter(PlayerData data)
    {
        data.playerMovement.stopMovement();
        Debug.Log("player is idling.");
    }
}

