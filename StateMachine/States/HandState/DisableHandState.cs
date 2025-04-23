using Data;

public class DisableHandState : HandState{

    public override void OnEnter(LedgerData data)
    {
       LedgerMovement.INSTANCE.StopAllCoroutines();
       LedgerMovement.INSTANCE.DisableHand();
        
    }

}