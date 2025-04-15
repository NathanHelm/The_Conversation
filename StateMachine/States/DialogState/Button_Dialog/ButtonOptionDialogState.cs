using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ButtonOptionDialogState : DialogueState{
    public override void OnEnter(DialogueData data)
    {
       Debug.Log("ButtonOptionDialogState");
       ButtonDialogueManager.INSTANCE.AddEventsToButtons();
       ButtonDialogueManager.INSTANCE.EnableButtons();


    }
    public override void OnUpdate(DialogueData data)
    {
        ButtonDialogueManager.INSTANCE.BetweenButtons();
    }
    public override void OnExit(DialogueData data)
    {
       ButtonDialogueManager.INSTANCE.RemoveEventsToButtons();
       ButtonDialogueManager.INSTANCE.HideButtons();
       ButtonDialogueManager.INSTANCE.R();

    }
    

   
}