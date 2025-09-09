using UnityEngine;
using System.Collections;

public class ClueMono : BodyMono
{
    //clue mono contains writing which will be added first.
    [SerializeField]
    private ClueScriptableObject clueScriptableObject;

    public string imageDescription;
    public int questionID; //unlocked question that is said the characters to reveal the plot... 
    public Texture ledgerImage;
    public Texture[] ledgerOverlays;

    public int clueQuestionID = 0; //dialogue id that describes the clue on the page.
    public int clueID = 0; 

    public static int clueBodyID { get; set; } = 31; //as things stand this should really stay as 31

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
            clueQuestionID = clueScriptableObject.clueQuestionID;
            clueID = clueScriptableObject.clueID;
            gameObject.layer = LayerMask.NameToLayer("cluecollider");
            bodyID = clueBodyID;
            base.OnEnable();
        }
    }
    public void SetClueScriptableObject(ClueScriptableObject clueScriptableObject) //function is mostly testing purposes 
    {
        this.clueScriptableObject = clueScriptableObject;
    }

}

