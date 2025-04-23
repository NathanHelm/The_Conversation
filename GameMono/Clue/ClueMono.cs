using UnityEngine;
using System.Collections;

public class ClueMono : BodyMono
{
    //clue mono contains writing which will be added first.
    [SerializeField]
    private ClueScriptableObject clueScriptableObject;

    public string imageDescription;
    public int questionID; //unlocked question that is said the characters to reveal the plot... 
	public int[] memoryId;
    public Texture ledgerImage;
    public Texture[] ledgerOverlays;

    public static int clueQuestionID;

    public DialogueConversation dialogueConversation;


    public override void OnEnable()
    {
        //note that dialogueconversation index = 0 will only run
        if (clueScriptableObject != null)
        {
            
            imageDescription = clueScriptableObject.imageDescription;
            ledgerImage = clueScriptableObject.ledgerImage;
            ledgerOverlays = clueScriptableObject.ledgerOverlays;
            questionID = clueScriptableObject.questionID;
            dialogueConversation = clueScriptableObject.dialogConversation;
            if(memoryId == null)
            {
               Debug.LogError("there are no memories attached to clue " + bodyID);
            }
            memoryId = clueScriptableObject.memoryId;

            gameObject.layer = LayerMask.NameToLayer("cluecollider");
            base.OnEnable();
        }
    }
    public void SetClueScriptableObject(ClueScriptableObject clueScriptableObject) //function is mostly testing purposes 
    {
        this.clueScriptableObject = clueScriptableObject;
    }

}

