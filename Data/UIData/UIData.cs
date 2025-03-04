using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
   
namespace Data
{
    public class UIData : StaticInstance<UIData>
    { 
        public Animator transitionAnimator { get; set; }
        public Image dialogBlock { get; set; }
        public TextMeshProUGUI dialogText { get; set; }

        public Button dialogueButton, dialogueButton1; 


        public override void OnEnable()
        {
            transitionAnimator = GetComponentInChildren<Animator>();
            dialogText = FindObjectOfType<TextMeshProUGUI>();
            dialogBlock = FindObjectOfType<Image>();
            var buttons = FindObjectsOfType<DialogueButton>();
            dialogueButton = buttons[0].b;
            dialogueButton1 = buttons[1].b;

            ButtonDialogueManager.INSTANCE.actionCall.AddAction((ButtonDialogueManager e)=>{e.button1 = dialogueButton; e.button2 = dialogueButton1;});

            base.OnEnable();
        }
      



    }
}

