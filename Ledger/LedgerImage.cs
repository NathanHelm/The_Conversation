using UnityEngine;
using System.Collections;
using Codice.ThemeImages;

public class LedgerImage
{
	public string imageDescription  {get;set;} = "";  //TODO maybe change this? 
	public int questionID {get;set;} //certain images unlocks questions that vet can ask to others.
    public int[] memoryId;
    public Texture ledgerImage { get; private set; }

    public Texture[] ledgerOverlays;

    public int clueQuestionID;
    
    public int clueBodyID; //set value to whatever you desire

    

    public LedgerImage(string imageDescription, int questionID, int clueQuestionID, Texture ledgerImg, Texture[] ledgerOverlays, int[] memoryID, int clueBodyID)
    {
        this.imageDescription = imageDescription;
        this.questionID = questionID;
        this.clueQuestionID = clueQuestionID;
        this.clueBodyID = clueBodyID;
        this.ledgerImage = ledgerImg;
        this.memoryId = memoryID;
        this.ledgerOverlays = ledgerOverlays;
    }
}

