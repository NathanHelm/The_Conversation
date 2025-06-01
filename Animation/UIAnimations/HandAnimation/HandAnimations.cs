using Codice.LogWrapper;
using Data;
using UnityEngine;

public enum HandAnimation{
    PointAnim, 
    FlipAnim,
    ClickAnim,
    WriteAnim,
    HoldPage,

    LHoldPage,

    LLastFlip
}
public class HandAnimations : StaticInstance<HandAnimations>{
    
    public Animator rightHandAnim {get; set;}
    public Animator leftHandAnim {get; set;}

    private GameObject leftHand;

    private Renderer leftHandPageShader;  //NOTE that varaible is only set replace image state.

    [SerializeField]
    HandScriptableObject handScriptableObject;

    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction(m => m.handAnimations = this);
        base.OnEnable();
    }
    public override void OnDisable()
    {
        MManager.INSTANCE.handAnimations = null;
        base.OnDisable();
    }

    public override void m_Start()
  {
    LedgerMovement.onAfterCreateHands.AddAction((LedgerMovement lm) =>
    {
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
        else if(handAnimation == HandAnimation.LHoldPage)
        {
            AnimationManager.INSTANCE.PlayAnimation(ref handAnim, "hold_page");
            AnimationManager.INSTANCE.ChangeAnimationSpeed(ref handAnim,speed);
        }
        
        else if(handAnimation == HandAnimation.LHoldPage)
        {
            handAnim = leftHandAnim;
            AnimationManager.INSTANCE.PlayAnimation(ref handAnim, "hold_page");
            AnimationManager.INSTANCE.ChangeAnimationSpeed(ref handAnim,speed);
        }
        else if(handAnimation == HandAnimation.LLastFlip)
        {
            handAnim = leftHandAnim;
            AnimationManager.INSTANCE.PlayAnimation(ref handAnim, "last_flip");
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

      leftHandPageShader = SetPageToTexture(ref leftHand, image, overlayTextures); //obtaining the renderer from the left hand enabled page...

      leftHandPage.SetActive(true);
      leftHandThumb.SetActive(true);
    }
    private Renderer SetPageToTexture(ref GameObject page,Texture image, Texture[] imageOverlays)
    {
      var test = page.GetComponentsInChildren<Renderer>();
      //this code sucks... so many children I aint even pregnent ha... hah ahaha.
      var overlayImage = page.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>();
      overlayImage.material.SetTexture("_MainTex", image);
      Debug.LogError("TODO: add page data to ledgerimage to depict correct page image and add overlay images in prefab");
  
      return overlayImage;
      //TODO frontPage -- add 
    }
    public void DisableLeftHandPage()
    {
      
       //TODO add action to onaftereraseimage -- add code below to action. 
      GameObject leftHandPage = leftHand.transform.GetChild(0).gameObject;
      GameObject leftHandThumb = leftHand.transform.GetChild(1).gameObject;
      leftHandPage.SetActive(false);
      leftHandThumb.SetActive(false);
    }
    public void EraseImageAnimationLeftHandPage()
    {
      PageAnimations.INSTANCE.EraseImage(leftHandPageShader);
    }
    public void DrawImageAnimationLeftHandPage()
    {
     PageAnimations.INSTANCE.DrawImage(leftHandPageShader);
    }
   

}