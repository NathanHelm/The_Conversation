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

    public int textureId;
    public int persistentDialogueId;

        public JsonDialogueConversationObject(List<DialogueConversation> dialogueObjects, int characterId/* int textureId, int persistentConversationId*/)
        {
            this.dialogueObjects = dialogueObjects;
            this.Id = characterId;
            /*
            this.textureId = textureId;
            this.persistentDialogueId = persistentConversationId;
            */
        }

}
}

