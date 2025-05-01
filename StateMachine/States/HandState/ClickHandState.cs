
using Data;
using UnityEngine;
public class ClickHandState : HandState{

    public override void OnEnter(LedgerData data)
    {
       HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.ClickAnim,LedgerData.INSTANCE.flipPageTime);
       
    }

}