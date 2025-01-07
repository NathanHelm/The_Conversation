using UnityEngine;
using System.Collections;

public class ClueMono : BodyMono
{
    //clue mono contains writing which will be added first.
    [SerializeField]
    private ClueScriptableObject clueScriptableObject;


    public static int questionId { get; set; } = 0;
	public DialogueConversation[] vetClueConversation { get; set; }

    public override void OnEnable()
    {
        //note that dialogueconversation index = 0 will only run
        vetClueConversation = clueScriptableObject.dialogueConversations;
        base.OnEnable();
    }

}

