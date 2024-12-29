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
            d.dialogueLineToAction = GetActionOnConversation(DialogueData.INSTANCE.currentCharacterID, DialogueData.INSTANCE.currentQuestionID);
        });
        base.OnEnable();
    }

    public void SetUpDialogueAction() //preprocess the dialogue action
    {
        //CharacterID   
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
           //QuestionID 
            { 0,
            new Dictionary<int, Action>() { 
            //index in the conversation
            { 1,
                ()=>{ Debug.Log("testing"); }
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

