using UnityEngine;
[System.Serializable]
public class JsonLedgerImageObject
{
    public string imageDescription = "";  //TODO maybe change this? 
    public int questionID; //certain images unlocks questions that vet can ask to others.
    public Texture ledgerImage;

    public Texture[] ledgerOverlays;

    public int clueQuestionID;

    public int clueBodyID = 31; //set value to whatever you desire

    public JsonLedgerImageObject(string imageDescription, int questionID, int clueQuestionID, Texture ledgerImg, Texture[] ledgerOverlays, int clueBodyID)
    {
        this.imageDescription = imageDescription;
        this.questionID = questionID;
        this.clueQuestionID = clueQuestionID;
        this.clueBodyID = clueBodyID;
        this.ledgerImage = ledgerImg;
        this.ledgerOverlays = ledgerOverlays;
    }
}