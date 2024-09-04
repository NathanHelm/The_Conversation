using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
   
namespace Data
{
    public class UIData : StaticInstance<UIData>
    { 
        public Animator transitionAnimator { get; set; }
        public Image image { get; set; }

        

        public override void Awake()
        {
            transitionAnimator = GetComponentInChildren<Animator>();
            GameEventManager.INSTANCE.AddEventFunc("uidata", GetData);
            base.Awake();
        }

        public UIData GetData()
        {
            return this;
        }
            
    }
}

