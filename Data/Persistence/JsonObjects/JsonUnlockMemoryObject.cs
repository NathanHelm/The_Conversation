namespace Persistence
{
    [System.Serializable]
    public class JsonUnlockMemoryObject : JsonObject
    {
        public int characterID;
        public MemoryObject[] memoryObjects;

        public JsonUnlockMemoryObject(int characterID, MemoryObject[] memoryObjects)
        {
            Id = this.characterID = characterID;
            this.memoryObjects = memoryObjects;
        }

    }
}
