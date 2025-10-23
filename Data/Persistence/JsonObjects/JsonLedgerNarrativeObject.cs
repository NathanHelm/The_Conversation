namespace Persistence
{
    [System.Serializable]
    public class JsonLedgerNarrativeObject : JsonObject
    {
        public int clueBodyID = 31; //set value to whatever you desire
        public int questionID;
        //AK clueBody + questionID
        public string ledgerImageStreamAssetFileName;
        public bool persistent = true;

        public JsonLedgerNarrativeObject(int clueBodyID, int questionID, string ledgerImageStreamAssetFileName, bool persistent)
        {
            this.clueBodyID = clueBodyID;
            this.questionID = questionID;
            this.ledgerImageStreamAssetFileName = ledgerImageStreamAssetFileName;
            this.persistent = persistent;
        }




    }
}