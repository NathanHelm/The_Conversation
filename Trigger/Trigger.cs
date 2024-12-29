using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{

    public List<BodyMono> bodiesOnTrigger { get; set; } = new List<BodyMono>();
    public List<CharacterMono> charactersOnTrigger { get; set; } = new List<CharacterMono>();
    public Collider triggerCol;


    public ActionRef2 triggerState;

    public delegate void ActionRef2(Collider col, ref Trigger trigger);

    public Action<Collider, Trigger> triggerExitState;
    public bool isPlayerOnTrigger { get; set; }
    private Trigger trigger;

    public void Start()
    {
        trigger = this;
      
    }

    public void GetCharacterOnTriggerCount()
    {

    }


    public void OnTriggerEnter(Collider other)
    {
        triggerCol = other;
       
        triggerState?.Invoke(triggerCol, ref trigger);

    }  
    public void OnTriggerExit(Collider other)
    {

        triggerCol = other;
        triggerExitState?.Invoke(triggerCol, this);
   
    }

}

