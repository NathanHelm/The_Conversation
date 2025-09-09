using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
using System.Linq;
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
            Debug.LogError("two characters are inside the trigger");
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
            Debug.LogError("two or more characters in trigger");
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
    public void ChangeTriggerExitState(Action<Collider2D, Trigger> newTrigger)
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

    public void DefaultTrigger(Collider2D other, ref Trigger trigger)
    {
        Debug.Log("TRIGGER--ENTER!" + other.gameObject.name);
        BodyMono[] bodyMono = other.GetComponents<BodyMono>();
        if (bodyMono.Length == 0)
        {
            return;
        }

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
        if (trigger.charactersOnTrigger.Count > 1)
        {
            Debug.LogWarning("2 or more characters on trigger");

            //-get characterIDs in trigger
            //-get trigger action for multiple triggers
            List<int> characterIDs = new();
            foreach (CharacterMono single in trigger.charactersOnTrigger)
            {
                characterIDs.Add(single.bodyID);
            }
            Data.TriggerData.INSTANCE.characterMonosInTrigger = trigger.charactersOnTrigger;
            triggerActionManager.GetTriggerActionMultipleTriggers(characterIDs.ToArray())();
            return;
        }
        

        if (trigger.charactersOnTrigger.Count > 0) //if player != null return
        {
            int characterOnTrigger = GetCharacterID(trigger);
            InterviewData.interviewTexturePath = trigger.charactersOnTrigger[0].interviewFaceStreamingAssetsImageName;
            dialogueData.currentCharacterID = characterOnTrigger;

                                                    //will change with further state changes
            dialogueData.currentQuestionID =
            dialogueData.currentPersistentConversationID = trigger.charactersOnTrigger[0].persistentConversationQuestionId;

            triggerActionManager.GetTriggerAction(characterOnTrigger)(); //will run a certain action based on character Id. 
        }
        if(trigger.bodiesOnTrigger.Count > 0)
        {
            int bodyOnTriggerId = GetBodyID(trigger);
            triggerActionManager.GetTriggerAction(bodyOnTriggerId)(); 
        }
    }



    //trigger exit state
    public void DefaultTriggerExit(Collider2D other, Trigger trigger)
    {

            Debug.Log("TRIGGER--EXIT!" + other.gameObject.name);
            BodyMono[] bodyMono = other.GetComponents<BodyMono>();
            if (bodyMono.Length == 0)
            {
            //has no body arribute, therefore it will not run trigger actions.
            return;
            }
            if (trigger.charactersOnTrigger.Count > 1)
            {
                Debug.LogWarning("2 or more characters on trigger");

                //-get characterIDs in trigger
                //-get trigger action for multiple triggers
                List<int> characterIDs = new();
                foreach (CharacterMono single in trigger.charactersOnTrigger)
                {
                    characterIDs.Add(single.bodyID);
                }
                Data.TriggerData.INSTANCE.characterMonosInTrigger = trigger.charactersOnTrigger;

                var tempList = new List<CharacterMono>();
                int max = trigger.charactersOnTrigger.Count - 1;
                //this code sucks
                while (max >= 0)
                {
                tempList.Add(trigger.charactersOnTrigger[max]);
                --max;
                }
                tempList.Remove(other.GetComponent<CharacterMono>());

                triggerActionManager.GetTriggerExitActionMultipleTriggers(characterIDs.ToArray(), tempList[0])();
                RemoveBodyMonos(ref other, ref trigger);
                RemoveCharacterMonos(ref other, ref trigger);
                return;
            }
        if (trigger.charactersOnTrigger.Count > 0) //if player != null return
        {
            int characterOnTriggerId = GetCharacterID(trigger);
            triggerActionManager.GetTriggerExitAction(characterOnTriggerId)();
        }

        if (trigger.bodiesOnTrigger.Count > 0) //is player
        {
            /*
            DialogueManager.INSTANCE.NoDialogue();
            GameEventManager.INSTANCE.OnEvent(typeof(NoConversationState));
            */

            int bodyOnTriggerId = GetBodyID(trigger);
            triggerActionManager.GetTriggerExitAction(bodyOnTriggerId)();

        }
        RemoveBodyMonos(ref other, ref trigger);
        RemoveCharacterMonos(ref other, ref trigger);
    }
    private void RemoveBodyMonos(ref Collider2D other, ref Trigger trigger)
    {
        if(other.GetComponent<BodyMono>() != null)
        {
            trigger.bodiesOnTrigger.Remove(other.GetComponent<BodyMono>());
        }
    }

    private void RemoveCharacterMonos(ref Collider2D other, ref Trigger trigger)
    {
        if (other.GetComponent<CharacterMono>() != null)
        {
            trigger.charactersOnTrigger.Remove(other.GetComponent<CharacterMono>());
        }
    }
    public void IdleTriggerEnterState(Collider2D other, ref Trigger trigger)
    {
        Debug.Log("trigger enter is in idle");
    }
    public void IdleTriggerExitState(Collider2D other, Trigger trigger)
    {
        Debug.Log("trigger exit is in idle");
    }







}

