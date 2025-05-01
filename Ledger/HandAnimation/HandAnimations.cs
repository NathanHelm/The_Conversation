using Data;
using UnityEngine;

public enum HandAnimation{
    PointAnim, 
    FlipAnim,
    ClickAnim,
    WriteAnim,
    HoldPage
}
public class HandAnimations : StaticInstance<HandAnimations>{
    
    public Animator rightHandAnim {get; set;}
    public Animator leftHandAnim {get; set;}

    private GameObject leftHand;

    [SerializeField]
    HandScriptableObject handScriptableObject;

    public override void m_Start()
    {
      LedgerMovement.INSTANCE.onAfterCreateHands.AddAction((LedgerMovement lm) => {
        rightHandAnim = LedgerData.INSTANCE.rightHandAnim;
        leftHandAnim = LedgerData.INSTANCE.leftHandAnim;
        leftHand = LedgerData.INSTANCE.leftHandObj;
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
        else if(handAnimation == HandAnimation.WriteAnim)
        {
            AnimationManager.INSTANCE.PlayAnimation(ref handAnim, "draw");
            AnimationManager.INSTANCE.ChangeAnimationSpeed(ref handAnim,speed);
        }
        else if(handAnimation == HandAnimation.HoldPage)
        {
            handAnim = leftHandAnim;
            AnimationManager.INSTANCE.PlayAnimation(ref handAnim, "hold_page");
            AnimationManager.INSTANCE.ChangeAnimationSpeed(ref handAnim,speed);
        }
    }
    public void FlipHand(bool isLeft)
    {
      Animator handAnim = rightHandAnim;
      handAnim.transform.eulerAngles = new Vector3(0, 0, 0);
      AnimationManager.INSTANCE.Flip(isLeft, ref handAnim);
    }
    public void SetLeftHandToImage(LedgerImage ledgerImage)
    {
      GameObject leftHandPage = leftHand.transform.GetChild(0).gameObject;
      GameObject leftHandThumb = leftHand.transform.GetChild(1).gameObject;

      Texture[] overlayTextures = ledgerImage.ledgerOverlays;
      Texture image = ledgerImage.ledgerImage;

      SetPageToTexture(ref leftHand, image, overlayTextures);

      leftHandPage.SetActive(true);
      leftHandThumb.SetActive(true);
    }
    private void SetPageToTexture(ref GameObject page,Texture image, Texture[] imageOverlays)
    {
      var frontPage = page.GetComponentInChildren<Renderer>();
      var overlayImage = frontPage.transform.GetChild(0).GetComponentInChildren<Renderer>();
      overlayImage.material.SetTexture("_MainTex", image);
      //TODO frontPage -- add 
      Debug.LogError("TODO: add image function to depict correct page image and add overlay images in prefab");
    }
    public void DisableLeftHandPage()
    {
      GameObject leftHandPage = leftHand.transform.GetChild(0).gameObject;
      GameObject leftHandThumb = leftHand.transform.GetChild(1).gameObject;
      leftHandPage.SetActive(false);
      leftHandThumb.SetActive(false);

    }
   

}