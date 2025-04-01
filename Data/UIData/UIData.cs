﻿using UnityEngine;
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
            dialogText = FindObjectOfType<UIDialogText>().GetComponent<TextMeshProUGUI>();
            dialogBlock = FindObjectOfType<Image>();
            var buttons = FindObjectsOfType<DialogueButton>();

            buttons[1].ID = 1; //setting button to true.
            buttons[0].ID = 2; //setting button to false.

            dialogueButton = buttons[1].GetComponent<Button>();
            dialogueButton1 = buttons[0].GetComponent<Button>();
           

            ButtonDialogueManager.onActionStart.AddAction((ButtonDialogueManager e)=>{e.button1 = dialogueButton; e.button2 = dialogueButton1;});

            base.OnEnable();
        }
      



    }
}

