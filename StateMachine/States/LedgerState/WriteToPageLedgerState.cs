using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;

public class WriteToPageLedgerState : LedgerState
{
    
    public override void OnEnter(LedgerData data)
    {
        //NOTE--> ledger image data has already BEEN added with omit raycast call. 
        Debug.Log("write to ledger");

        ActionController.AFTERPAGEFLIP_LEDGER -= ActionController.INSTANCE.afterFlipBehaviour.pointActionLedgerMovement;

        
        /*
        LedgerManager.INSTANCE.movePageLeft.RemoveAction(
        LedgerData.INSTANCE.pointActionLedgerManager
        );
        LedgerManager.INSTANCE.movePageRight.RemoveAction(
        LedgerData.INSTANCE.pointActionLedgerManager
        );
        */

        LedgerManager.INSTANCE.subject.RemoveObserver(LedgerData.INSTANCE);
        LedgerManager.INSTANCE.subject.RemoveObserver(LedgerData.INSTANCE);

        GameEventManager.INSTANCE.OnEvent(typeof(EnableHandState));
        

        /* TODO add if needed
         LedgerMovement.onAfterFlipAwait.AddAction(
            LedgerData.INSTANCE.writeActionLedgerMovement
        );
        */

        LedgerManager.INSTANCE.WriteToPageInLedger();
       

       
        /*
        LedgerManager.onMovePageLeft.AddAction(
            LedgerData.INSTANCE.writeActionLedgerManager
        );
        LedgerManager.onMovePageRight.AddAction(
            LedgerData.INSTANCE.writeActionLedgerManager
        );
        */

       


        
        //TODO Add on event to ledgerdata, call it when drawing animation is done.
       // GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));

        //do writing animation 
        
        
        //TODO do drawing animation here.
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
        //return back to hand point state, remove current write state 
        ActionController.AFTERPAGEFLIP_LEDGER -= ActionController.INSTANCE.afterFlipBehaviour.writeActionLedgerMovement;
        ActionController.AFTERPAGEFLIP_LEDGER += ActionController.INSTANCE.afterFlipBehaviour.pointActionLedgerMovement;

        
        LedgerManager.INSTANCE.subject.AddObserver(LedgerData.INSTANCE);
        LedgerManager.INSTANCE.subject.AddObserver(LedgerData.INSTANCE);

    }
}
