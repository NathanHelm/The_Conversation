using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{

    public List<BodyMono> bodiesOnTrigger { get; set; } = new List<BodyMono>();
    public List<CharacterMono> charactersOnTrigger { get; set; } = new List<CharacterMono>();
    public Collider2D triggerCol;


    public ActionRef2 triggerState;

    public delegate void ActionRef2(Collider2D col, ref Trigger trigger);

    public Action<Collider2D, Trigger> triggerExitState;
    public bool isPlayerOnTrigger { get; set; }
    private Trigger trigger;

    public void Start()
    {
        trigger = this;
      
    }

    public void GetCharacterOnTriggerCount()
    {

    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        triggerCol = other;
       
        triggerState?.Invoke(triggerCol, ref trigger);

    }  
    public void OnTriggerExit2D(Collider2D other)
    {

        triggerCol = other;
        triggerExitState?.Invoke(triggerCol, this);
   
    }

}

