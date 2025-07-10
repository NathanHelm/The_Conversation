using System.Diagnostics;
using ActionControl;
using Data;
using UnityEngine;

public class InspectClueLedgerState : LedgerState
{
  public override void OnEnter(LedgerData data)
  {
    UnityEngine.Debug.Log("inspect clue state!");
    GameEventManager.INSTANCE.OnEvent(typeof(HoldPageState));

    var ledgerUIImage = UIData.INSTANCE.ledgerUIImage.gameObject;
    var renderer = UIData.INSTANCE.ledgerUIImage;
    UIManager.INSTANCE.EnableUIObject(ref ledgerUIImage);
    LedgerImageManager.INSTANCE.SetRenderTextureToLedgerImage(ref renderer, data.pageObjectsIndex);
    ImageUIAnimations.INSTANCE.DrawLedgerImageUI();

    ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabDisableLedger;
    ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopDialogue;
    ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopCutscene;

  }
    public override void OnUpdate(LedgerData data)
    {
      /*
    if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab))
    {
      ActionController.PRESSTAB(LedgerManager.INSTANCE);
    }

        base.OnUpdate(data);
        */
    }
    public override void OnExit(LedgerData data)
  {

    ImageUIAnimations.INSTANCE.EraseLedgerImageUI();
    var ledgerUIImage = UIData.INSTANCE.ledgerUIImage.gameObject;
    UIManager.INSTANCE.DisableUIObject(ref ledgerUIImage);

    ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabDisableLedger;
    ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopDialogue;
    ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabStopCutscene;

    base.OnExit(data);
  }
}