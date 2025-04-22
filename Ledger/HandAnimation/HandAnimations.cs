using Data;
using UnityEngine;

public enum HandAnimation{
    PointAnim, 
    FlipAnim,
    ClickAnim,
}
public class HandAnimations : StaticInstance<HandAnimations>{
    
    public Animator rightHandAnim {get; set;}
    public Animator leftHandAnim {get; set;}

    [SerializeField]
    HandScriptableObject handScriptableObject;

    public override void m_Start()
    {
      LedgerMovement.INSTANCE.onAfterCreateHands.AddAction((LedgerMovement lm) => {
        rightHandAnim = LedgerData.INSTANCE.rightHand;
        leftHandAnim = LedgerData.INSTANCE.leftHand;
      });
        
    }
    public void PlayHandAnimation(HandAnimation handAnimation, float speed)
    {
        Animator handAnim = rightHandAnim;
        
        if(handAnimation == HandAnimation.PointAnim)
        {
            AnimationManager.INSTANCE.PlayAnimation(ref handAnim,"point");
            AnimationManager.INSTANCE.ChangeAnimationSpeed(ref handAnim,speed);
        }
        else if(handAnimation == HandAnimation.ClickAnim)
        {
             AnimationManager.INSTANCE.PlayAnimation(ref handAnim, "click");
             AnimationManager.INSTANCE.ChangeAnimationSpeed(ref handAnim,speed);
        }
        else if(handAnimation == HandAnimation.FlipAnim)
        {
             AnimationManager.INSTANCE.PlayAnimation(ref handAnim, "flip");
             AnimationManager.INSTANCE.ChangeAnimationSpeed(ref handAnim,speed);
        }
    }
    public void FlipHand(bool isLeft)
    {
      Animator handAnim = rightHandAnim;
      handAnim.transform.eulerAngles = new Vector3(0, 0, 0);
      AnimationManager.INSTANCE.Flip(isLeft, ref handAnim);
    }

}