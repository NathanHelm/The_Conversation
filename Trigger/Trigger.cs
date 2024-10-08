﻿using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;

public class Trigger : MonoBehaviour
{
    public TriggerData triggerData;
    public List<BodyMono> bodiesOnTrigger { get; set; } = new List<BodyMono>();
    public List<CharacterMono> charactersOnTrigger { get; set; } = new List<CharacterMono>();
    public Collider triggerCol;
    public Action<Collider, Trigger> triggerState, triggerExitState;
    public bool isPlayerOnTrigger { get; set; }

    public void Start()
    {
        triggerData = GameEventManager.INSTANCE.OnEventFunc<TriggerData>("data.triggerdata");
    }


    public void OnTriggerEnter(Collider other)
    {
        triggerCol = other;
        triggerState?.Invoke(triggerCol, this);

    }
    public void OnTriggerExit(Collider other)
    {

        triggerCol = other;
        triggerExitState?.Invoke(triggerCol, this);
   
    }

}

