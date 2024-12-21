using UnityEngine;
using System.Collections;
using Data;
public class ConversationState : DialogueState
{
    //runs the conversation between a detective and character
    //player questions lead to differenct responses via the key pair in class "Question Response Manager."
    public override void OnEnter(DialogueData data)
    {
        DialogueManager.INSTANCE.StartConversation(data); 
    }
    public override void OnExit(DialogueData data)
    {
       
    }
    public override void OnUpdate(DialogueData data)
    {
        DialogueManager.INSTANCE.RunDialog();
    }
}

