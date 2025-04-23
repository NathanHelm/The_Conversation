using Data;
using UnityEngine;
public class PointHandState : HandState{
    public override void OnEnter(LedgerData data)
    {
       if(!LedgerData.INSTANCE.isLedgerCreated)
       {
       LedgerMovement.INSTANCE.EnableHand();
       }
       LedgerMovement.INSTANCE.StopAllCoroutines();
       LedgerMovement.INSTANCE.HandPointAtPage();
    }
     
}