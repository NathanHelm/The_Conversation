using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Persistence{
[Serializable]
public class JsonDialogueConversationObject : JsonObject
{
    [SerializeField]
    public List<DialogueConversation> dialogueObjects;

    public JsonDialogueConversationObject(List<DialogueConversation> dialogueObjects, int characterId)
    {
        this.dialogueObjects = dialogueObjects;
        this.Id = characterId;
    }

}
}

