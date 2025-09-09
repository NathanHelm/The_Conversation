using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ImmediateConversationState : DialogueState
{
    /*conversation is triggered right away!*/
    /*Note! add */
    public override void OnEnter(DialogueData data)
    {
        GameEventManager.INSTANCE.OnEvent(typeof(TransitionTo3d));

        if (DialogueManager.INSTANCE != null)
        {
            /*
            - This is super important and unique to immediate conversation...
            - We will only run action end dialog conversation if there is no other subscribed action
            - This is because states like conversationselectionstate uses this state in action trigger but the after dialogue action changed to something else.
            */
            if (ActionController.AFTERDIALOGUE.GetInvocationList().Length == 0)
            {
                ActionController.AFTERDIALOGUE += ActionController.INSTANCE.actionEndDialogue.runEndDialogConversation;//run clue!
            }
                DialogueManager.INSTANCE.StartConversation(DialogueData.INSTANCE);
            DialogueManager.INSTANCE.RunDialogueNoInput();
        }
    }
    public override void OnUpdate(DialogueData data)
    {
        
        if (DialogueManager.INSTANCE != null)
        {
          
            DialogueManager.INSTANCE.RunDialog();
        }
    }
    public override void OnExit(DialogueData data)
    {
        if (DialogueManager.INSTANCE != null)
        {
            if (ActionController.AFTERDIALOGUE.GetInvocationList().Length == 0)
            {
                ActionController.AFTERDIALOGUE -= ActionController.INSTANCE.actionEndDialogue.runEndDialogConversation;//run clue!
            }
            // DialogueManager.INSTANCE.onEndDialogue.RemoveAction(ActionController.INSTANCE.actionEndDialogue.runEndDialogConversation); //switchstate
        }
    }
}
