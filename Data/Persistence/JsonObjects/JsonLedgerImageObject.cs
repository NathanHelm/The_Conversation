using UnityEngine;
[System.Serializable]
public class JsonLedgerImageObject
{
    public string imageDescription = "";  //TODO maybe change this? 
    public int questionID; //certain images unlocks questions that vet can ask to others.
    public Texture ledgerImage;

    public Texture[] ledgerOverlays;

    public int clueQuestionID;

    public int clueID;

    public int clueBodyID = 31; //set value to whatever you desire
    
    //Alternate key==============================
    public SceneNames sceneName;
    public int clueCameraID; 
    //============================================


    public JsonLedgerImageObject(int clueID, string imageDescription, int questionID, int clueQuestionID, Texture ledgerImg, Texture[] ledgerOverlays, int clueBodyID)
    {
        this.clueID = clueID;
        this.imageDescription = imageDescription;
        this.questionID = questionID;
        this.clueQuestionID = clueQuestionID;
        this.clueBodyID = clueBodyID;
        this.ledgerImage = ledgerImg;
        this.ledgerOverlays = ledgerOverlays;
    }
}