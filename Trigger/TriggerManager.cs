﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class TriggerManager : StaticInstance<TriggerManager>
{

    public TriggerData triggerData;
    public Trigger[] triggers;

    public void SetUpTrigger() //runs on enter dialog states
    {
       triggerData = GameEventManager.INSTANCE.OnEventFunc<TriggerData>("data.triggerdata");
       
       triggers = triggerData.triggers;
    }

    public int GetBodyID(Trigger trigger) //returns the body id that collided with the trigger.
    {
        if (trigger.triggerCol == null)
        {
            throw new NullReferenceException("trigger doesn't have a trigger collider");
        }

        if (trigger.bodiesOnTrigger.Count > 1)
        {
            throw new Exception("two characters are inside the trigger");
        }

        return trigger.bodiesOnTrigger[0].bodyID;

    }



    public int GetCharacterID(Trigger trigger) //return the character id based on what object collided with it 
    {

        if (trigger.triggerCol == null)
        {
            throw new NullReferenceException("trigger doesn't have a trigger collider");
        }

        if (trigger.charactersOnTrigger.Count > 1)
        {
            throw new Exception("two characters are inside the trigger");
        }

       
        return trigger.charactersOnTrigger[0].bodyID; //negative id values symbolize something went wrong
 

        throw new NullReferenceException("characters is not found");
    }

    public void ChangeTriggerStates(Action<Collider, Trigger> newTrigger)
    {
        //sets ALL trigger to a new state.
        foreach (Trigger trigger in triggers)
        {
            trigger.triggerState = newTrigger;
        }
    }
    public void ChangeTriggerExitState(Action<Collider, Trigger> newTrigger)
    {
       foreach(Trigger trigger in triggers)
        {
            trigger.triggerExitState = newTrigger;
        }
    }



    //trigger states, the default trigger
    //first adds either body or character to the '___ onTrigger ' list
    //then checks if the player is in trigger and there is a also a character in the list
    //if both is true, set the data's current character to the character thats in the trigger.
    //also sets the current question id to be the character on trigger's persistent id (in case you set the dialog action to basic dialog, no questions)

    public void DefaultTrigger(Collider other, Trigger trigger)
    {
        PlayerLook player = other.GetComponent<PlayerLook>();
        BodyMono bodyMono = other.GetComponent<BodyMono>();

        triggerData.triggerOnTrigger = trigger;
       

        if (bodyMono != null) //adds collider which is in trigger to a list
        {
            trigger.bodiesOnTrigger.Add(bodyMono);
        }
        if (other.GetComponent<CharacterMono>() != null)
        {
            trigger.charactersOnTrigger.Add(other.GetComponent<CharacterMono>());
        }


        if (player != null && trigger.charactersOnTrigger.Count > 0) //if player != null return
        {
            int characterOnTrigger = triggerData.triggerManager.GetCharacterID(trigger);
            triggerData.dialogueData.currentCharacterID = characterOnTrigger;

                                                    //will change with further state changes
            triggerData.dialogueData.currentQuestionID =
            triggerData.dialogueData.currentPersistentConversationID = trigger.charactersOnTrigger[0].persistentConversationId;

            triggerData.triggerActionManager.GetTriggerAction(characterOnTrigger)();
        }
        if(player != null && trigger.bodiesOnTrigger.Count > 0)
        {
            int bodyOnTriggerId = triggerData.triggerManager.GetBodyID(trigger);
            triggerData.triggerActionManager.GetTriggerAction(bodyOnTriggerId)(); 
        }
    }

    

    //trigger exit state
    public void DefaultTriggerExit(Collider other, Trigger trigger)
    {
        PlayerLook player = other.GetComponent<PlayerLook>();
        if (player != null) //is player
        {
            GameEventManager.INSTANCE.OnEvent(typeof(NoConversationState));
            return;
        }

    }







}

