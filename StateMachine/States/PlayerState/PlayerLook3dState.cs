using UnityEngine;
using System.Collections;
using Data;
public class PlayerLook3dState : PlayerState
{
    public override void OnEnter(PlayerData data)
    {
        data.playerLook.mouseSensitivity = data.currentPlayerSO.cameraSensitivity;
        data?.playerLook.SetOpeningPlayerRotation();
        Debug.Log("player look 3d state");
    }
    public override void OnUpdate(PlayerData data)
    {
        data.playerLook.LookContorl();
       
    }
}

