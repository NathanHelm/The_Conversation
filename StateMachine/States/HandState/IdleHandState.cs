
using Data;
using UnityEngine;

public class IdleHandState : HandState{

    public override void OnEnter(LedgerData data)
    {
        Debug.Log("idle hand state"); 
    }

}