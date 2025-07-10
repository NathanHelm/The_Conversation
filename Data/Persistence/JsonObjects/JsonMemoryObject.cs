using UnityEngine;

namespace Persistence
{
    [System.Serializable]
    public class JsonMemoryObject : JsonObject
    {
        //TODO change the representation of the Json memory object so that is holds an array of different memories and sub memories.
        public int characterID;
        public MemoryObject[] memoryObject;

        public JsonMemoryObject(int id, MemoryObject[] memoryObject)
        {
            this.memoryObject = memoryObject;
            characterID = Id = id;
        }
    }
}