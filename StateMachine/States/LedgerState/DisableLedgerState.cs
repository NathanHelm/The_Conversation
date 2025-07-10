using UnityEngine;
using System.Collections;
using Data;

public class DisableLedgerState : LedgerState
{
    public override void OnEnter(Data.LedgerData data)
    {
        Debug.Log("entered disable ledger state");


        if (!LedgerData.INSTANCE.isLedgerCreated)
        {
          LedgerManager.INSTANCE.CreateLedger();
        }
        LedgerManager.INSTANCE.DisableLedger();

        ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabStartCutscene;
        ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabOpenLedger;
       
        base.OnEnter(data);
    }
  public override void OnUpdate(Data.LedgerData data)
  {
    /*
    if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab)) //NOTE THIS WILL CHANGE DEPENDING ON A ID 2 is in trigger or not.
    {
      UnityEngine.Debug.Log("HELP!");
      ActionController.PRESSTAB(LedgerManager.INSTANCE);
    }
    */
  }
  public override void OnExit(LedgerData data)
  {
      ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabOpenLedger;
      ActionController.PRESSTAB_LEDGER -= ActionController.INSTANCE.actionOpenLedgerTab.pressTabStartCutscene;
  }
}

