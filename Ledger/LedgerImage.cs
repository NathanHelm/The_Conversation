using UnityEngine;
using System.Collections;
using Codice.ThemeImages;
[System.Serializable]
public class LedgerImage
{
	public string imageDescription  {get;set;} = "";  //TODO maybe change this? 
	public int questionID {get;set;} //certain images unlocks questions that vet can ask to others.
    public Texture ledgerImage { get; private set; }

    public Texture[] ledgerOverlays;

    public int clueQuestionID;
    
    public int clueBodyID = 31; //set value to whatever you desire

    

    public LedgerImage(string imageDescription, int questionID, int clueQuestionID, Texture ledgerImg, Texture[] ledgerOverlays, int clueBodyID)
    {
        this.imageDescription = imageDescription;
        this.questionID = questionID;
        this.clueQuestionID = clueQuestionID;
        this.clueBodyID = clueBodyID;
        this.ledgerImage = ledgerImg;
        this.ledgerOverlays = ledgerOverlays;
    }
}

