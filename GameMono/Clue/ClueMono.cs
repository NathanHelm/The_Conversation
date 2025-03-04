using UnityEngine;
using System.Collections;

public class ClueMono : BodyMono
{
    //clue mono contains writing which will be added first.
    [SerializeField]
    private ClueScriptableObject clueScriptableObject;

    public string imgDescription { get; set; }
    public int[] clueQuestions { get; set; } 
    public int[] memories {get; set;}

	public DialogueConversation[] vetClueConversation { get; set; }

    public override void OnEnable()
    {
        //note that dialogueconversation index = 0 will only run
        if (clueScriptableObject != null)
        {
            vetClueConversation = clueScriptableObject.dialogueConversations;
            imgDescription = clueScriptableObject.imgDescription;
            clueQuestions = clueScriptableObject.clueQuestions;
            if(memories == null)
            {
               Debug.LogError("there are no memories attached to clue " + bodyID);
            }
            memories = clueScriptableObject.memories;

            gameObject.layer = LayerMask.NameToLayer("cluecollider");
            base.OnEnable();
        }
    }
    public void SetClueScriptableObject(ClueScriptableObject clueScriptableObject) //function is mostly testing purposes 
    {
        this.clueScriptableObject = clueScriptableObject;
    }

}

