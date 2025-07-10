using UnityEngine;
using System;
namespace ActionControl
{
    public class ActionEndDialogue
    {
    public Action<DialogueManager> runEndDialogConversation = lm =>
    {
        GameEventManager.INSTANCE.OnEvent(typeof(EndConversationReplayState));
    };
    public Action<DialogueManager> runEndClueInspection = lm =>
    {
        GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState));
        GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));
    };

    }
}