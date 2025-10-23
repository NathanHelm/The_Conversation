[System.Serializable]
public class LedgerNarrativeObject
{
    //public static clueBodyID = 31 {get;set;} //set value to whatever you desire
    public int questionID;
    //AK clueBody + questionID
    public string ledgerImageStreamAssetFileName;

    public bool persistent = true;

    public LedgerNarrativeObject(int questionID, string ledgerImageStreamAssetFileName, bool persistent)
    {
        this.questionID = questionID;
        this.ledgerImageStreamAssetFileName = ledgerImageStreamAssetFileName;
        this.persistent = persistent;
    }

 

}
