using UnityEngine;
using System.Collections;
using Data;
public class ConversationState : DialogueState
{
    //runs the conversation between a detective and character
    //player questions lead to differenct responses via the key pair in class "Question Response Manager."
    public override void OnEnter(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
            DialogueManager.onEndDialogue.AddAction(DialogueData.INSTANCE.runEndDialogConversation); //switchstate

            DialogueManager.INSTANCE.StartConversation(data);
        }

    }
    public override void OnExit(DialogueData data)
    {
       DialogueManager.onEndDialogue.RemoveAction(DialogueData.INSTANCE.runEndDialogConversation); //switchstate
      
    }
    public override void OnUpdate(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
            DialogueManager.INSTANCE.RunDialog();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            //TODO --> initiate interview state...
            
            Debug.Log("InterviewState!");
        }
    }
}

