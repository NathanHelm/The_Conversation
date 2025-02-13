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

        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Debug.Log("mouse position" + mousePos);

        PlayerData.INSTANCE.playerRaycast.OmitRaycast(mousePos);

        CutsceneManager.INSTANCE.SetCutSceneActionAndTime(new (Action, float)[]
        {
            new (()=>{ },0f),
        new (()=> { }, .5f),
        new (()=> {}, .5f)

        }
        );

        CutsceneManager.INSTANCE.SetSnapShot(new (string, Type)[] { new("DimensionState", typeof(TransitionTo3d)), new("PlayerState", typeof(PlayerLook3dState)), new ("DialogueState",typeof(NoConversationState)) });
        GameEventManager.INSTANCE.OnEvent(typeof(PlayCutsceneState));

        //0) player clicks on clue!

        

        //1) data exchange, add data to ledger data / dialog data based on click

        //2) play animation based on image.  
        //3) play dialog based on image. (set character in dialogue action)
        //4) walk again (switch state)
    }

}

