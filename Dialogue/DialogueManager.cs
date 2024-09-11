using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Data;
public class DialogueManager : StaticInstance<DialogueManager>
{

   private int dialogueIndex = 0;
   private int dialogueID;
   private int questionID;
   bool isDialogueScrolling;
   DialogueObject[] dialogueObjects;
   Dictionary<int, DialogueAction> dialogueLineToAction;
   

   private TextMeshProUGUI textMesh;

   private DialogueConversation currentdialogueConvo;

   


   public void StartConversation(DialogueData dialogueData)
   {
        //runs on enter
        AddDialogueAction(); //gets the dialogue action
        dialogueID = dialogueData.characterID;
        questionID = dialogueData.questionID;
        currentdialogueConvo = dialogueData.currentConversation;
        dialogueObjects = currentdialogueConvo.dialogueObjects;
 
   }
    //conversation state
    public void RunDialog()
    {
      

        if(Input.GetKeyDown(KeyCode.Return) && !isDialogueScrolling)
        {
            if (dialogueIndex > dialogueObjects.Length - 1)
            {
                StopAllCoroutines();
                GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState)); //switchstate
                return;
            }   
            StartCoroutine(DialogueScroll(dialogueObjects[dialogueIndex]));

        }



    }



    IEnumerator DialogueScroll(DialogueObject dialogueObject)
    {
        isDialogueScrolling = true;

        textMesh.text = "";

        DialogueObject currentdialogue = dialogueObject; //gets line before key is replaced.

        string line = currentdialogue.line;
 
        for (int i = 0; i < line.Length; i++)
        {
            char singleText = line[i];
            textMesh.text += singleText;

            yield return new WaitForSeconds(0.02f);
        }

        PlayDialogAction(dialogueIndex); //plays dialogue action.

        ++dialogueIndex;

        isDialogueScrolling = false;

        yield return null;
    }

    private void PlayDialogAction(int index)
    {
        if (dialogueLineToAction == null)
        {
            return;
        }
        if (dialogueLineToAction[index] == null)
        {
            return;
        }
        Action[] dialogueAction = dialogueLineToAction[index].actions;
        foreach(Action single in dialogueAction)
        {
            single(); //runs the action based on the dialogue index.
        }
    }
    //endconversationstate
    public void RunDialogAgain()
    {
        if(Input.GetKeyDown(KeyCode.Return)) //plays the dialog again
        {
            GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));
        }
    }
    //noconversationstate
    public void NoDialogue()
    {
        StopAllCoroutines();
        Debug.Log("no dialog");
    }


    private void AddDialogueAction()
    {
        dialogueLineToAction = GameEventManager.INSTANCE.OnEventFunc<int, Dictionary<int, DialogueAction>>("dialogueactionmanager", dialogueID);
    }

    
 


}
