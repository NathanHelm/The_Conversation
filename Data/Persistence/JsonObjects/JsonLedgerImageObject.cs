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
    
    //clue camera data==============================
    public SceneNames sceneName;

    public int clueCameraForeignKey;
    //============================================
    // public LedgerImage(int clueID, string imageDescription, int questionID, int clueQuestionID, Texture ledgerImg, Texture[] ledgerOverlays, int clueBodyID, int ledgerImageKey)
   
    public JsonLedgerImageObject(string imageDescription, int questionID, Texture ledgerImage, Texture[] ledgerOverlays, int clueQuestionID, int clueID, int clueBodyID, SceneNames sceneName, int clueCameraForeignKey)
    {
        this.imageDescription = imageDescription;
        this.questionID = questionID;
        this.ledgerImage = ledgerImage;
        this.ledgerOverlays = ledgerOverlays;
        this.clueQuestionID = clueQuestionID;
        this.clueID = clueID;
        this.clueBodyID = clueBodyID;
        this.sceneName = sceneName;
        this.clueCameraForeignKey = clueCameraForeignKey;
    }
    



}