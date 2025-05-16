using System.Diagnostics;
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
    PageAnimations.INSTANCE.DrawLedgerImageUI();

    

  }
    public override void OnUpdate(LedgerData data)
    {
      if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab))
      {
      GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
      GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));
      }

        base.OnUpdate(data);
    }
    public override void OnExit(LedgerData data)
  {

    PageAnimations.INSTANCE.EraseLedgerImageUI();
    var ledgerUIImage = UIData.INSTANCE.ledgerUIImage.gameObject;
    UIManager.INSTANCE.DisableUIObject(ref ledgerUIImage);

    base.OnExit(data);
  }
}