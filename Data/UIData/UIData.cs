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


        public override void OnEnable()
        {
            transitionAnimator = GetComponentInChildren<Animator>();
            dialogText = FindObjectOfType<TextMeshProUGUI>();
            dialogBlock = FindObjectOfType<Image>();
            base.OnEnable();
        }



    }
}

