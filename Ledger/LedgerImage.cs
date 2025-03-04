using UnityEngine;
using System.Collections;

public class LedgerImage
{
	public string imageDescription  {get;set;} = ""; 
	public int[] customQuestions {get;set;} //certain images unlocks questions that vet can ask to others.
    public int bodyID {get; set;}

    public int[] memoryId;
    public Sprite ledgerImageSprite { get; private set; }
    

    public LedgerImage(string imageDescription, int[] customQuestions, int bodyID, Sprite ledgerImageSprite, int[] memoryID)
    {
        this.imageDescription = imageDescription;
        this.customQuestions = customQuestions;
        this.bodyID = bodyID;
        this.ledgerImageSprite = ledgerImageSprite;
        this.memoryId = memoryID;
    }
}

