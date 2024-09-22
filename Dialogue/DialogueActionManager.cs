using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
public class DialogueActionManager : StaticInstance<DialogueActionManager>
{
    public Dictionary<int, Dictionary<int, Dictionary<int, DialogueAction>>> conversationKeyToDialogActions =

    new Dictionary<int, Dictionary<int, Dictionary<int, DialogueAction>>>();

    public override void m_Start()
    {
        GameEventManager.INSTANCE.AddEventFunc<int, int, Dictionary<int, DialogueAction>>("getactiononconversation", GetActionOnConversation);

        SetUpDialogueAction();

    }
    public override void Awake()
    {
        base.Awake();
     
    }
    public void SetUpDialogueAction() //preprocess the dialogue action
    {
                                    //CharacterID   
        conversationKeyToDialogActions.Add(1, new Dictionary<int, Dictionary<int, DialogueAction>>()
        {
           //QuestionID 
            { 3,
            new Dictionary<int, DialogueAction>() { 
            //index in the conversation
            { 1, new DialogueAction(new Action[] {

                ()=>{ GameEventManager.INSTANCE.AddEvent("", ()=>{ });  }

                

            })




            }
            }
        }
        });
                                    //2 = vet
        conversationKeyToDialogActions.Add(2, new Dictionary<int, Dictionary<int, DialogueAction>>()
        {
           //QuestionID 
            { 0,
            new Dictionary<int, DialogueAction>() { 
            //index in the conversation
            { 1, new DialogueAction(new Action[] {

                ()=>{ GameEventManager.INSTANCE.AddEvent(typeof(EndConversationState), ()=>{

                     DialogueData triggerData = GameEventManager.INSTANCE.OnEventFunc<DialogueData>("data.dialoguedata");
                     triggerData.currentPersistentConversationID = 13;
                     GameEventManager.INSTANCE.ReplaceWithPrevEvent("endconversationstate");
                });  }



            })




            }
            }
        }
        });
    }






    public Dictionary<int, DialogueAction> GetActionOnConversation(int characterID, int questionID)
    {
        //getactiononconversation(dialoguedata.characterID, dialoguedata.questionid)
        if(questionID.Equals(""))
        {
            throw new KeyNotFoundException("key is nothing");
        }

        if (!conversationKeyToDialogActions.ContainsKey(characterID))
        {
            Debug.Log("no character action found for id" + characterID);
            return new Dictionary<int, DialogueAction>();
        }
        if(!conversationKeyToDialogActions[characterID].ContainsKey(questionID))
        {
            Debug.Log("no question id found for id" + questionID);
            return new Dictionary<int, DialogueAction>();
           
        }
        return conversationKeyToDialogActions[characterID][questionID];

    }

}

