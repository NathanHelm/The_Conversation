using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class ReplaceLedgerState : LedgerState
{
  /*
  essentially, we are replacing the 'image data'/page data from one page and replacing it with a new image
  
  */
    public override void OnEnter(LedgerData data)
    {
      LedgerManager.INSTANCE.MovePagesFurthestRight();
      //enabling page and thumb, adding the right textures too.
      //TODO change this
      HandAnimations.INSTANCE.SetLeftHandToImage(/*LedgerImageManager.INSTANCE.temporaryImage*/ Tester_Script.temporaryImageTest);
      HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.HoldPage, 1f);
    }
    public override void OnUpdate(LedgerData data)
    {
        LedgerManager.INSTANCE.MovePages();
        LedgerManager.INSTANCE.ReplacePage();
        LedgerImageManager.INSTANCE.SetTemporaryImageToNull();
        if(Input.GetKeyDown(KeyCode.Tab))
        {
        GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
        GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));
        GameEventManager.INSTANCE.OnEvent(typeof(PlayCutsceneState));
        }
    }
    public override void OnExit(LedgerData data)
    {
        HandAnimations.INSTANCE.DisableLeftHandPage();
    }

}
