using Data;

public class MoveHandFlipState : HandState{

    public override void OnEnter(LedgerData data)
    {
        LedgerMovement.INSTANCE.MoveHand();
    }

}