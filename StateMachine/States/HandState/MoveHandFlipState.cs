using Data;

public class MoveHandFlipState : HandState{

    public override void OnEnter(LedgerData data)
    {
       LedgerMovement.INSTANCE.MoveHandAwaitPoint();
    }
    public override void OnExit(LedgerData data)
    {
       
    }

}