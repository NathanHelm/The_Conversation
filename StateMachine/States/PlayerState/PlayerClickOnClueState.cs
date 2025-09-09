using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class PlayerClickOnClueState : PlayerState
{
    public override void OnEnter(PlayerData data)
    {
        Debug.Log("on player click on clue state");

        var direction = PlayerData.INSTANCE.trans3d.rotation * Vector3.forward;



        CutsceneManager.INSTANCE.SetOPreviousState(new (string, Type)[] { new("DimensionState", typeof(TransitionTo3d)), new("PlayerState", typeof(PlayerLook3dState)), new("DialogueState", typeof(NoConversationState)) });

        CutsceneManager.INSTANCE.RemovePreviousStateMono("CutsceneState");
        CutsceneManager.INSTANCE.RemoveStopStateMono("CutsceneState");

        CutsceneManager.INSTANCE.RemoveStopStateMono("LedgerState");
        CutsceneManager.INSTANCE.RemovePreviousStateMono("LedgerState");

        CutsceneManager.INSTANCE.RemoveStopStateMono("HandState");
        CutsceneManager.INSTANCE.RemovePreviousStateMono("HandState");
        
        
        PlayerData.INSTANCE.playerRaycast.OmitRaycast(direction);


        
        //0) player clicks on clue!



        //1) data exchange, add data to ledger data / dialog data based on click

        //2) play animation based on image.  
        //3) play dialog based on image. (set character in dialogue action)
        //4) walk again (switch state)
    }

}

