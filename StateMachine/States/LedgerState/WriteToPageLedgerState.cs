using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class WriteToPageLedgerState : LedgerState
{
    public override void OnEnter(LedgerData data)
    {
        Debug.Log("write to ledger");
        LedgerManager.INSTANCE.WriteToPageInLedger();
        //TODO do drawing animation here.
        LedgerManager.INSTANCE.DisableLedger();
        //LedgerManager.INSTANCE.
    }
    public override void OnUpdate(LedgerData data)
    {
        //0) start cutscene. --done already
        //1) if images index > page index (10) move to replace state.
        
       //2) go to images index 
       //3) if press d : replace state 
       //4) flip to page image index
       //5) use function setpage in ledgeruimanager. (run animation?)
       //6) close journal.
       //7) close cutscene (return to previous states...)
       

    }
    public override void OnExit(LedgerData data)
    {
       
    }
}
