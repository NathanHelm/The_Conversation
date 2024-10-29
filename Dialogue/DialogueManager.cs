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
   private int questionID;
   private int characterID;
   bool isDialogueScrolling = false;
   DialogueObject[] dialogueObjects;

   Dictionary<int, DialogueAction> dialogueLineToAction; //dialog action

   private DialogueConversation currentdialogueConvo; //dialog string, img, ... ect

   UIManager uIManager;

 
   public void StartConversation(DialogueData dialogueData)
   {
       
        characterID = dialogueData.currentCharacterID; 
        questionID = dialogueData.currentQuestionID; //If the trigger state is switched and the character ID doesn't have a custom trigger action, it will set currentQuestionID to persistentconversationID. 
        AddDialogueConversation();
        AddDialogueAction();
        dialogueObjects = currentdialogueConvo.dialogueObjects;
        uIManager = dialogueData.uIManager;

    }
   
    public void RunDialog()
    {
         
        if(Input.GetKeyDown(KeyCode.Return) && !isDialogueScrolling)
        {
            ShowDialogUIAndDialogScroll();
        }

    }

    public void ShowDialogUIAndDialogScroll()
    {
        if (dialogueIndex == 0)
        {
            uIManager.EnableDialogUI();
        }
        if (dialogueIndex > dialogueObjects.Length - 1)
        {

            GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState)); //switchstate
            return;
        }
        StartCoroutine(DialogueScroll(dialogueObjects[dialogueIndex]));

    }

    IEnumerator DialogueScroll(DialogueObject dialogueObject)
    {


        isDialogueScrolling = true;

        uIManager.ResetText();

        DialogueObject currentdialogue = dialogueObject; //gets line before key is replaced.

        string line = currentdialogue.line;
 
        for (int i = 0; i < line.Length; i++)
        {
            char singleText = line[i];
            uIManager.GetDialogText().text += singleText;

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
        if (!dialogueLineToAction.ContainsKey(index))
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
       
         
        if (Input.GetKeyDown(KeyCode.Return)) //plays the dialog again
        {
            ShowDialogUIAndDialogScroll();
            GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));
        }
    }
    //noconversationstate
    public void NoDialogue()
    {
        StopAllCoroutines();
        uIManager.DisableDialogUI();
        dialogueIndex = 0;
        Debug.Log("no dialog");
    }


    private void AddDialogueConversation()
    {
        currentdialogueConvo = GameEventManager.INSTANCE.OnEventFunc<int,int, DialogueConversation>("getdialogueconversation", characterID, questionID);
    }

    private void AddDialogueAction()
    {
        dialogueLineToAction = GameEventManager.INSTANCE.OnEventFunc<int, int, Dictionary<int, DialogueAction>>("getactiononconversation", characterID, questionID);
    }

    
 


}
