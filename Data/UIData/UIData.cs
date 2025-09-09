using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
   
namespace Data
{
    public class UIData : StaticInstanceData<UIData>, IExecution
    { 
        public Animator transitionAnimator { get; set; }
        public Image dialogBlock { get; set; }
        public TextMeshProUGUI dialogText { get; set; }

        public Renderer ledgerUIImage {get; set;}
        public Renderer interviewIcon {get; set;}

        public Button dialogueButton, dialogueButton1; 


        public override void m_OnEnable()
        {
            transitionAnimator = GetComponentInChildren<Animator>();
            dialogText = FindObjectOfType<UIDialogText>().GetComponent<TextMeshProUGUI>();
            dialogBlock = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<Image>();
            var buttons = FindObjectsOfType<DialogueButton>();

            ledgerUIImage = GameObject.FindGameObjectWithTag("LedgerImage")?.GetComponent<Renderer>();
            interviewIcon = GameObject.FindGameObjectWithTag("InterviewIcon")?.GetComponent<Renderer>();

           


            buttons[1].ID = 1; //setting button to true.
            buttons[0].ID = 2; //setting button to false.

            dialogueButton = buttons[1].GetComponent<Button>();
            dialogueButton1 = buttons[0].GetComponent<Button>();



           
            IconUIAnimations.onStartImageUIAnimations.AddAction(iuia => { iuia.drawLedgerUIPageIndexRenderer = ledgerUIImage;});

            ButtonDialogueManager.onActionStart.AddAction((ButtonDialogueManager e)=>{e.button1 = dialogueButton; e.button2 = dialogueButton1;});
            

            base.m_OnEnable();
        }
      



    }
}

