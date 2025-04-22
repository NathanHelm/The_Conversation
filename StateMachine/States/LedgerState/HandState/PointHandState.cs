using Data;

public class PointHandState : HandState{
    public override void OnEnter(LedgerData data)
    {
       LedgerMovement.INSTANCE.EnableHand();
       LedgerMovement.INSTANCE.HandPointAtPage();
    }
     
}