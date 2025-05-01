using Data;

public class DisableHandState : HandState{

    public override void OnEnter(LedgerData data)
    {
       LedgerMovement.INSTANCE.StopMoveRecentState();
       LedgerMovement.INSTANCE.moveToPosStack = new();
       LedgerMovement.INSTANCE.DisableHand();
       HandAnimations.INSTANCE.DisableLeftHandPage();
        
    }

}