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
    GameEventManager.INSTANCE.OnEvent(typeof(EnableHandState));

    ActionController.AFTERPAGEFLIP_LEDGER -= ActionController.INSTANCE.afterFlipBehaviour.writeActionLedgerMovement;


    LedgerManager.INSTANCE.MovePagesFurthestRight();

    //enabling page and thumb, adding the right textures too.
    //TODO change this
    HandAnimations.INSTANCE.SetLeftHandToImage(LedgerImageManager.INSTANCE.temporaryImage);
    HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.LHoldPage, 1f);
    HandAnimations.INSTANCE.DrawImageAnimationLeftHandPage();
      
    ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopCutscene;
    ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabDisableLedger;
     
      
  }
    public override void OnUpdate(LedgerData data)
    {
        LedgerManager.INSTANCE.MovePages();
        LedgerManager.INSTANCE.ReplacePage();
        /*
        if(Input.GetKeyDown(KeyCode.Tab))
        {
          ActionController.PRESSTAB(LedgerManager.INSTANCE);
        }
        */
    }
    public override void OnExit(LedgerData data)
    {
      ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopCutscene;
      ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabDisableLedger;
     
      
    }

}
