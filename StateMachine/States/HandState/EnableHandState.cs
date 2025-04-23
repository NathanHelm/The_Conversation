using Data;

public class  EnableHandState : HandState{

    public override void OnEnter(LedgerData data)
    {
       LedgerMovement.INSTANCE.EnableHand();
    }

}