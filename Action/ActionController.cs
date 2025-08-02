using System.Collections;
using System.Collections.Generic;
using System;
using Data;
using UnityEngine;
using ActionControl;

public class ActionController : StaticInstance<ActionController>, IExecution
{
    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager mm) => { mm.actionController = this; });
    }
    public override void m_Start()
    {
        AFTERPAGEFLIP_LEDGER += afterFlipBehaviour.pointActionLedgerMovement;
        base.m_Start();
    }

    /*

    Action controls are a list of organized actions that can be subscribed to our static actions
    
    */
    //Dialogue Actions============================================================================================================================================
    //end dialogue

    //Ledger Actions=============================================================================================================================================================
    public ActionControl.ActionEndDialogue actionEndDialogue = new();
    public ActionControl.ActionOpenLedgerTab actionOpenLedgerTab = new();
    public ActionControl.ActionOpenLedgerSelectPage actionOpenLedgerSelectPage = new();
    public ActionControl.ActionRunDialogue actionRunDialogue = new();
    public ActionControl.AfterFlipBehaviour afterFlipBehaviour = new();

    public ActionControl.FragAction<LedgerMovement> fragActionLedgerManager = new();

    //=============================================================================================================================================================

    public static Action<LedgerManager> PRESSTAB_LEDGER;
    public static Action<LedgerManager> PRESSRETURN_LEDGER;
    public static Action<LedgerMovement> AFTERPAGEFLIP_LEDGER;
    public static Action<LedgerManager> AFTERPAGEFLIPFURTHESTLEFT_LEDGER;


    public void Update()
    {

        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab)) //NOTE THIS WILL CHANGE DEPENDING ON A ID 2 is in trigger or not.
        {
            PRESSTAB_LEDGER(LedgerManager.INSTANCE);
        }

    }
    public void PressReturn()
    {
       
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return))
        {
            //TODO add this to conversation state.
            PRESSRETURN_LEDGER(LedgerManager.INSTANCE);
        }
    }
    

}
