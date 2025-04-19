using UnityEngine;
using System.Collections;

public class LedgerImage
{
	public string imageDescription  {get;set;} = ""; 
	public int[] customQuestions {get;set;} //certain images unlocks questions that vet can ask to others.
    public int bodyID {get; set;}

    public int[] memoryId;
    public Texture ledgerImage { get; private set; }
    

    public LedgerImage(string imageDescription, int[] customQuestions, int bodyID, Texture ledgerImg, int[] memoryID)
    {
        this.imageDescription = imageDescription;
        this.customQuestions = customQuestions;
        this.bodyID = bodyID;
        this.ledgerImage = ledgerImg;
        this.memoryId = memoryID;
    }
}

