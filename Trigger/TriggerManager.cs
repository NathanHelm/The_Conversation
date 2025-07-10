using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class TriggerManager : StaticInstance<TriggerManager>, IExecution
{
    public static SystemActionCall<TriggerManager> onStartTriggerManagerAction = new SystemActionCall<TriggerManager>();
    public static SystemActionCall<TriggerManager> onTriggerTriggerManagerAction = new SystemActionCall<TriggerManager>();

    public TriggerActionManager triggerActionManager { get; set; }
    public TriggerData triggerData { get; set; } //only reason for trigger data is to set variable triggerOnTrigger.
    public DialogueData dialogueData { get; set; }
    public Trigger[] triggers; //trigger data sets this variable.

    

    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) => { m.triggerManager = this; });
        base.m_OnEnable();
    }
    public override void m_Start()
    {
        onStartTriggerManagerAction.RunAction(this);
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

    public void ChangeTriggerStates(Trigger.ActionRef2 newTrigger)
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

    public void DefaultTrigger(Collider other, ref Trigger trigger)
    {

        string playerTagName = other.gameObject.tag;
        BodyMono[] bodyMono = other.GetComponents<BodyMono>();

        triggerData.triggerOnTrigger = trigger;
       
        for(int i = 0; i < bodyMono.Length; i++)
        {
        if (bodyMono[i] is not CharacterMono) //adds collider which is in trigger to a list
        {
            trigger.bodiesOnTrigger.Add(bodyMono[i]);
            break;
        }
        }
        if (other.GetComponent<CharacterMono>() != null) 
        {
            trigger.charactersOnTrigger.Add(other.GetComponent<CharacterMono>()); //note that player is NOT character.
        }
        if(trigger.charactersOnTrigger.Count > 1)
        {
            throw new Exception("2 or more characters on trigger");
        }
        

        if (playerTagName == "Player" && trigger.charactersOnTrigger.Count > 0) //if player != null return
        {
            int characterOnTrigger = GetCharacterID(trigger);

            dialogueData.currentCharacterID = characterOnTrigger;

                                                    //will change with further state changes
            dialogueData.currentQuestionID =
            dialogueData.currentPersistentConversationID = trigger.charactersOnTrigger[0].persistentConversationId;

            triggerActionManager.GetTriggerAction(characterOnTrigger)(); //will run a certain action based on character Id. 
        }
        if(playerTagName == "Player" && trigger.bodiesOnTrigger.Count > 0)
        {
            int bodyOnTriggerId = GetBodyID(trigger);
            triggerActionManager.GetTriggerAction(bodyOnTriggerId)(); 
        }
    }

    

    //trigger exit state
    public void DefaultTriggerExit(Collider other, Trigger trigger)
    {
        PlayerLook player = other.GetComponent<PlayerLook>();

        if (player != null && trigger.charactersOnTrigger.Count > 0) //if player != null return
        {
            int characterOnTriggerId = GetCharacterID(trigger);
            triggerActionManager.GetTriggerExitAction(characterOnTriggerId)();
        }
        
         if (player != null && trigger.bodiesOnTrigger.Count > 0) //is player
            {
                /*
                DialogueManager.INSTANCE.NoDialogue();
                GameEventManager.INSTANCE.OnEvent(typeof(NoConversationState));
                */

                int bodyOnTriggerId = GetBodyID(trigger);
                triggerActionManager.GetTriggerExitAction(bodyOnTriggerId)();

            }
       

        if (other.GetComponent<CharacterMono>() != null)
        {
            trigger.charactersOnTrigger.Remove(other.GetComponent<CharacterMono>());
        }
        if(other.GetComponent<BodyMono>() != null)
        {
            trigger.bodiesOnTrigger.Remove(other.GetComponent<BodyMono>());
        }

    }
   
    public void IdleTriggerEnterState(Collider other, ref Trigger trigger)
    {
        Debug.Log("trigger enter is in idle");
    }
    public void IdleTriggerExitState(Collider other, Trigger trigger)
    {
        Debug.Log("trigger exit is in idle");
    }







}

