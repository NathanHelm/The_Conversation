using Data;

public class HoldPageState : HandState{
    public override void OnEnter(LedgerData data)
    {
        LedgerMovement.INSTANCE.MoveHandToUILedgerImage();
        HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.ClickAnim,LedgerData.INSTANCE.flipPageTime);

        base.OnEnter(data);
    }
    public override void OnExit(LedgerData data)
    {
      
        base.OnExit(data);
    }
}