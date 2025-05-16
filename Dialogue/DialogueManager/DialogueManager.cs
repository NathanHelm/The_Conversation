using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Data;
public class DialogueManager : StaticInstance<DialogueManager>
{
    public static SystemActionCall<DialogueManager> actionOnStartConversation = new SystemActionCall<DialogueManager>();
    public static SystemActionCall<DialogueManager> actionOnAfterRunDialog = new SystemActionCall<DialogueManager>();

    public static SystemActionCall<DialogueManager> onEndDialogue = new SystemActionCall<DialogueManager>();


    private int dialogueIndex = 0;
    public string currentLine { get; set; }
    private bool isDialogueScrolling = false;


    public DialogueObject[] dialogueObjects { get; set; } //dialog string, img, ... ect
    public Dictionary<int, Action> dialogueLineToAction { get; set; } //dialog action

    private IEnumerator dialogScroll = null;


    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager m) => { m.dialogueManager = this; /*add m_start action*/ }); //dialogemanager has priority as other managers RELY on the dialogue manager.
    }


    public void StartConversation(DialogueData dialogueData)
    {
        //runs on dialogue state (on enter)
        if(actionOnStartConversation != null)
        {
            Debug.Log("conversation state");
            actionOnStartConversation.RunAction(this);  // sets your code for you! (sort of)
        }
        //If the trigger state is switched and the character ID doesn't have a custom trigger action, it will set currentQuestionID to persistentconversationID. 
       // dialogueObjects = actionManagerImplement.GetDialogueConversation(characterID, questionID) ;
       // dialogueLineToAction = qresponseMImplement.getDialogueAction();
    }

    public void RunDialog()
    {

        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return) && !isDialogueScrolling)
        {
            GameEventManager.INSTANCE.OnEvent(typeof(PlayerIdleState)); //player is idle
            ShowDialogUIAndDialogScroll();
        }

    }
    public void RunDialogueNoInput()
    {
        //TODO we should use this code for the interview state.
        GameEventManager.INSTANCE?.OnEvent(typeof(PlayerIdleState)); //player is idle
        UIManager.INSTANCE.EnableDialogUI();
        ShowDialogUIAndDialogScroll();
    }

    public void ShowDialogUIAndDialogScroll()
    {
        if (dialogueIndex == 0)
        {
            UIManager.INSTANCE.EnableDialogUI();
        }
        
        if (dialogueIndex > dialogueObjects.Length - 1)
        {
            onEndDialogue.RunAction(this);
            return;
        }
        StartCoroutine(dialogScroll = DialogueScroll(dialogueObjects[dialogueIndex]));

    }

    IEnumerator DialogueScroll(DialogueObject dialogueObject)
    {


        isDialogueScrolling = true;

        UIManager.INSTANCE.ResetText();

        DialogueObject currentdialogue = dialogueObject; //gets line before key is replaced.

        string line = currentLine = currentdialogue.line;

        for (int i = 0; i < line.Length; i++)
        {
            char singleText = line[i];
            UIManager.INSTANCE.GetDialogText().text += singleText;

            yield return new WaitForSeconds(0.02f);
        }

        PlayDialogAction(dialogueIndex); //plays dialogue action.

        ++dialogueIndex;

        isDialogueScrolling = false;

        yield return null;
    }

    private void PlayDialogAction(int index)
    {
        actionOnAfterRunDialog.RunAction(this);
        if (dialogueLineToAction == null)
        {
            return;
        }
        if (!dialogueLineToAction.ContainsKey(index))
        {
            return;
        }
        Action dialogueAction = dialogueLineToAction[index];
        dialogueAction();
    }
    //endconversationstate
    public void RunDialogAgain()
    {
        ShowDialogUIAndDialogScroll();
        GameEventManager.INSTANCE.OnEvent(typeof(ConversationState));
    }
    //noconversationstate
    public void NoDialogue()
    {
        if(dialogScroll != null)
        {
        StopCoroutine(dialogScroll);
        }
    
        UIManager.INSTANCE.DisableDialogUI();
        dialogueIndex = 0;
    }
    public void PlayerMove()
    {
        if(PlayerData.INSTANCE == null)
        {
            Debug.LogError("There is no player instance");
            return;
        }

        GameEventManager.INSTANCE.OnEvent(typeof(PlayerLook3dState));
    }



}
