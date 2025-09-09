using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class DialogueActionManager : StaticInstance<DialogueActionManager>, IExecution
{
    public Dictionary<int, Dictionary<int, Dictionary<int, Action>>> conversationKeyToDialogActions =

    new Dictionary<int, Dictionary<int, Dictionary<int, Action>>>();


    public override void m_Start()
    {

        SetUpDialogueAction();
      

    }
    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) =>
        {
            m.dialogueActionManager = this;
        });
            DialogueManager.actionOnStartConversation.AddAction((DialogueManager d) => {
            /* set code goes here*/
            //gets dialogie line based on character and question id. 
            d.dialogueLineToAction = GetActionOnConversation(DialogueData.INSTANCE.currentCharacterID, DialogueData.INSTANCE.currentQuestionID);
        });
        base.m_OnEnable();
    }

    public void SetUpDialogueAction() //preprocess the dialogue action
    {
        //CharacterID
        /*
         * currently with default trigger state, we obtain the character when, the character in on a trigger withe a character.
         *we get the dialog question id based on the 'Persistent Dialog Id : int' todo change 'persistent dialog id to vet's question based on ledger images.'
         */
        conversationKeyToDialogActions.Add(1, new Dictionary<int, Dictionary<int, Action>>()
        {
           //QuestionID 
            { 3,
            new Dictionary<int, Action>() { 
            //index in the conversation
            { 1,

                ()=>{  }

            }

            }

        }
        });
        //character id example     
        conversationKeyToDialogActions.Add(2, new Dictionary<int, Dictionary<int, Action>>()
        {
           //QuestionID -- the 'response' to a paticular question id, character id determines which lists what
           //question options are avaiable based on the character. 
            { 0,
            new Dictionary<int, Action>() { 
            //index in the conversation
            { 1, ()=>{


            }

              //  ()=>{Debug.Log("On index 1, play button option dialog"); GameEventManager.INSTANCE.OnEvent(typeof(ButtonOptionDialogState));
            }

            }
            }
        });
        conversationKeyToDialogActions.Add(21, new Dictionary<int, Dictionary<int, Action>>()
        {
            {
                46,//qid
                new(){
                    {
                        0, ()=> {
                            Debug.Log("unlock dialog");
                            MemoryManager.INSTANCE.UnlockMemory(21, 1);
                            MemoryManager.INSTANCE.UnlockSubMemory(21, 1);


                    }
                }
                }
            }
        });
        conversationKeyToDialogActions.Add(33, new Dictionary<int, Dictionary<int, Action>>()
        {
            {
                2,
                new Dictionary<int, Action>(){
                    {
                        3, ()=> {Debug.Log("AHA! -- you unlocked a memory conDRAGulations");}
                    }
                }
            }
        });
        conversationKeyToDialogActions.Add(210, new Dictionary<int, Dictionary<int, Action>>()
        {
            {
                2, //question 
                new Dictionary<int, Action>(){
                    { 0,

                    ()=>{ Debug.Log("210 has been triggered!"); 
                        //setting action to button manager.
                        /*
                            ButtonDialogueManager.INSTANCE.SetActions(()=> {
                            //open journal
                            //todo ==> add ledger state based on currentcharacterId. 

                            int currentCharacter = DialogueData.INSTANCE.currentCharacterID;

                            Debug.Log("you said YES!");

                           // GameEventManager.INSTANCE.OnEvent(typeof(NoConversationState));
                            GameEventManager.INSTANCE.OnEvent(typeof(TransitionTo2d));
                            GameEventManager.INSTANCE.OnEvent(typeof(PlayerLook3dState));
                            GameEventManager.INSTANCE.OnEvent(typeof(EndConversationReplayState));


                        }, ()=>{
                            //play dialogue? 
                            Debug.Log("you said No!");
                           // GameEventManager.INSTANCE.OnEvent(typeof(ImmediateConversationState));
                           //GameEventManager.INSTANCE.OnEvent()
                            GameEventManager.INSTANCE.OnEvent(typeof(TransitionTo2d));
                            GameEventManager.INSTANCE.OnEvent(typeof(PlayerLook3dState));
                            GameEventManager.INSTANCE.OnEvent(typeof(EndConversationReplayState));
                        });

                        GameEventManager.INSTANCE.OnEvent(typeof(ButtonOptionDialogState));
                        */
                     }

                    }

            }
            }
        });
        conversationKeyToDialogActions.Add(24, new Dictionary<int, Dictionary<int, Action>>()
        {
            { 46,
            new Dictionary<int, Action>() { 
            //index in the conversation
            { 1, ()=>{

                Debug.Log("index 1!");

            }
            }

            }
            }
        });
        


    }






    public Dictionary<int, Action> GetActionOnConversation(int characterID, int questionID)
    {
        //based on character id, question dictionary.
        //based on the question id, get the 

      
        if (!conversationKeyToDialogActions.ContainsKey(characterID))
        {
            Debug.Log("no character action found for id" + characterID);
            return new Dictionary<int, Action>();
        }
        if(!conversationKeyToDialogActions[characterID].ContainsKey(questionID))
        {
            Debug.Log("no question id found for id" + questionID);
            return new Dictionary<int, Action>();
           
        }
        return conversationKeyToDialogActions[characterID][questionID];

    }

   
}

