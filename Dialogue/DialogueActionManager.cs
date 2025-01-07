using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class DialogueActionManager : StaticInstance<DialogueActionManager>
{
    public Dictionary<int, Dictionary<int, Dictionary<int, Action>>> conversationKeyToDialogActions =

    new Dictionary<int, Dictionary<int, Dictionary<int, Action>>>();


    public override void m_Start()
    {

        SetUpDialogueAction();
      

    }
    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager m) =>
        {
            m.dialogueActionManager = this;
        });
            DialogueManager.actionOnStartConversation.AddAction((DialogueManager d) => {
            /* set code goes here*/
            //gets dialogie line based on character and question id. 
            d.dialogueLineToAction = GetActionOnConversation(DialogueData.INSTANCE.currentCharacterID, DialogueData.INSTANCE.currentQuestionID);
        });
        base.OnEnable();
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

                ()=>{ GameEventManager.INSTANCE.AddEvent("", ()=>{ });  }

            }

            }
            
        }
        });
                                    //2 = vetID
        conversationKeyToDialogActions.Add(2, new Dictionary<int, Dictionary<int, Action>>()
        {
           //QuestionID -- the 'response' to a paticular question id, character id determines which lists what
           //question options are avaiable based on the character. 
            { 0,
            new Dictionary<int, Action>() { 
            //index in the conversation
            { 1,
                ()=>{ Debug.Log("testing"); }
            }
            
            }
        }
        });
        conversationKeyToDialogActions.Add(3, new Dictionary<int, Dictionary<int, Action>>()
        {
            {
                0, //question 
                new Dictionary<int, Action>(){
                    { 0,

                    ()=>{ Debug.Log("test");}

                    }

            }
            }
        });
    }






    public Dictionary<int, Action> GetActionOnConversation(int characterID, int questionID)
    {
        //based on character id, question dictionary.
        //based on the question id, get the 

        if(questionID.Equals(""))
        {
            throw new KeyNotFoundException("key is nothing");
        }

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

