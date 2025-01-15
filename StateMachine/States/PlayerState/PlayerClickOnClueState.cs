using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;
public class PlayerClickOnClueState : State<PlayerData>
{
    public override void OnEnter(PlayerData data)
    {
        Debug.Log("on player click on clue state");

        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Debug.Log("mouse position" + mousePos);

        PlayerData.INSTANCE.playerRaycast.OmitRaycast(mousePos);


        //0) player clicks on clue!

        

        //1) data exchange, add data to ledger data / dialog data based on click

        //2) play animation based on image. (hold)
        //3) play dialog based on image. (set character in dialogue action)
        //4) walk again (switch state)
    }

}

