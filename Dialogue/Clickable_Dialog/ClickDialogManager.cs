using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;
using Data;
public class ClickDialogManager : StaticInstance<ClickDialogManager>
{
    public static SystemActionCall<ClickDialogManager> actionOnStartConversation = new SystemActionCall<ClickDialogManager>();
    private string currentDialogueLine;
    private TextMeshProUGUI textMeshProUGUI;
    


    private void SetUpCurrentDialogueData(DialogueManager dialogueManager)
    {
        currentDialogueLine = dialogueManager.currentLine;
    }
   

    public void SetUpClickData(DialogueManager dialogueManager)
    {
        this.currentDialogueLine = dialogueManager.currentLine;
    }
    

    public void ClickableDialogueAction()
	{
        ParseText(currentDialogueLine);
	}
    private void ParseText(string dialogueLine)
    {
        string[] words = dialogueLine.Split(" ");
        for(int i = 0; i < words.Length; i++)
        {
            words[i] = AddTags(dialogueLine);
        }
    }
    private string AddTags(string word)
    {
        return " <link> " + word + " <link> "; //todo make this functionalit with the textmeshpro
    }
}

