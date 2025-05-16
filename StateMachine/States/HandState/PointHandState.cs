using Data;
using UnityEngine;
public class PointHandState : HandState{
    public override void OnEnter(LedgerData data)
    {
       
       LedgerMovement.INSTANCE.StopMoveRecentState();
       LedgerMovement.INSTANCE.moveToPosStack = new();
       LedgerMovement.INSTANCE.HandPointAtPage();  
    }
    public override void OnUpdate(LedgerData data)
    { 
       
    }
    public override void OnExit(LedgerData data)
    {
        base.OnExit(data);
    }
     
}