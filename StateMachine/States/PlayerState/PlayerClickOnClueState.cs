using UnityEngine;
using System.Collections;
using Data;
public class PlayerClickOnClueState : State<PlayerData>
{
    public override void OnEnter(PlayerData data)
    {
        //0) player clicks on clue!

        //1) data exchange, add data to ledger data / dialog data based on click

        //2) play animation based on image. (hold)
        //3) play dialog based on image. (set character in dialogue action)
        //4) walk again (switch state)
    }

}

