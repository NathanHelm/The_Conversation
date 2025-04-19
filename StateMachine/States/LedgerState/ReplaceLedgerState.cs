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
      LedgerManager.INSTANCE.ReplacePageToLedger();
    }
    public override void OnUpdate(LedgerData data)
    {
        LedgerManager.INSTANCE.MovePages();
        LedgerManager.INSTANCE.ReplacePage();
    }
    public override void OnExit(LedgerData data)
    {
        
    }

}
