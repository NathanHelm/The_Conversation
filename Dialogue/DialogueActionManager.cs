using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class DialogueActionManager : StaticInstance<DialogueActionManager>
{
    public Dictionary<int, Dictionary<int, Dictionary<int, DialogueAction>>> conversationKeyToDialogActions =

    new Dictionary<int, Dictionary<int, Dictionary<int, DialogueAction>>>();

    public void Start()
    {
        SetUpDialogueAction();

        GameEventManager.INSTANCE.AddEventFunc<int,int, Dictionary<int, DialogueAction>>("getactiononconversation", GetActionOnConversation);


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
    }






    public Dictionary<int, DialogueAction> GetActionOnConversation(int characterID, int questionID)
    {

        if (questionID.Equals("") || !conversationKeyToDialogActions.ContainsKey(questionID))
        {
            throw new KeyNotFoundException("either key isn't there or the key was not found");
        }
        return conversationKeyToDialogActions[characterID][questionID];

    }

}

