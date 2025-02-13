using UnityEngine;
using System.Collections;

public class LedgerImage
{
	private string imageDescription = ""; 
	private (int, int) characterAndQuestionId; //character id and question interaction to run dialog (recall that question is almost always equal to 0)
	private int[] customQuestions; //certain images unlocks questions that vet can ask to others.
    private int bodyID;
    public Sprite ledgerImageSprite { get; private set; }
   

    public LedgerImage(string imageDescription, (int, int) characterAndQuestionId, int[] customQuestions, int bodyID, Sprite ledgerImageSprite)
    {
        this.imageDescription = imageDescription;
        this.characterAndQuestionId = characterAndQuestionId;
        this.customQuestions = customQuestions;
        this.bodyID = bodyID;
        this.ledgerImageSprite = ledgerImageSprite;
    }
}

