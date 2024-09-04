using UnityEngine;
using System.Collections;
using Data;
public class PlayerLook3dState : State<PlayerData>
{
    public override void OnEnter(PlayerData data)
    {
        data.playerLook.mouseSensitivity = data.currentPlayerSO.cameraSensitivity;
    }
    public override void OnUpdate(PlayerData data)
    {
        data.playerLook.LookContorl();
       
    }
}

