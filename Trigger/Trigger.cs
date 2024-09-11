using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
    public TriggerData triggerData;
    public List<BodyMono> bodiesOnTrigger { get; set; } = new List<BodyMono>();
    public List<CharacterMono> charactersOnTrigger { get; set; } = new List<CharacterMono>();
    public Collider triggerCol;
    public bool isPlayerOnTrigger { get; set; }

    public void Start()
    {
        triggerData = GameEventManager.INSTANCE.OnEventFunc<TriggerData>("data.triggerdata");
        
        
    }


    public void OnTriggerEnter(Collider other)
    {
         PlayerLook player = other.GetComponent<PlayerLook>();


        if (other.GetComponent<BodyMono>() != null) //adds collider which is in trigger to a list
        {
            bodiesOnTrigger.Add(other.GetComponent<BodyMono>()); 
        }
        if (other.GetComponent<CharacterMono>() != null)
        {
            charactersOnTrigger.Add(other.GetComponent<CharacterMono>());
        }

        if(player != null && charactersOnTrigger.Count > 0) //if player != null return
        {
            int characterOnTrigger = triggerData.triggerManager.GetCharacterID(other, this);
            triggerData.dialogueData.characterID = characterOnTrigger;
            triggerData.dialogueData.persistentConversationID = charactersOnTrigger[0].persistentConversationId;
            triggerData.triggerActionManager.GetTriggerAction(characterOnTrigger)();
          
        }
             
        
        
    }
    public void OnTriggerExit(Collider other)
    {
        
        bodiesOnTrigger.Remove(other.GetComponent<BodyMono>());
        /*
        BodyExiting

        if( != null)
        {
            charactersOnTrigger.Remove()
        }
        */

       // GameEventManager.INSTANCE.OnEvent(typeof());

    }

}

