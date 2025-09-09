using UnityEngine;
using System.Collections;
using Data;
/*
same functionality as player look 3d state except the cursor wont reset on enter!

*/
public class Playerlook3dNoCursorWrap : PlayerState
{
    public override void OnEnter(PlayerData data)
    {
        //data?.playerLook.Look();
        Debug.Log("player look 3d state, no cursor was set");
    }
    public override void OnUpdate(PlayerData data)
    {
        data.playerLook.LookContorl();
       
    }
}

