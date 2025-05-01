using Data;
using UnityEngine;
public class WriteHandState : HandState{
    public override void OnEnter(LedgerData data)
    {
       LedgerMovement.INSTANCE.StopMoveRecentState();
       LedgerMovement.INSTANCE.HandWriting();
    }
    public override void OnExit(LedgerData data)
    {
        Animator anim = HandAnimations.INSTANCE.rightHandAnim;
       AnimationManager.INSTANCE.StopAnimation(ref anim);
    }
}