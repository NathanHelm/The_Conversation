using System.Collections;
using System.Collections.Generic;
using System;
using Data;
using UnityEngine;
using ActionControl;
using ObserverAction;
using System.Linq;
using Codice.CM.Common;

public class ActionController : StaticInstance<ActionController>, IExecution, IObserverData<PlayerActions, ClueMono>
{
    /*

Action controls are a list of organized actions that can be subscribed to our static actions

*/
    //Dialogue Actions============================================================================================================================================
    public ActionControl.ActionEndDialogue actionEndDialogue = new();  //end dialogue
   

    //Ledger Actions=============================================================================================================================================================
    public ActionControl.ActionOpenLedgerTab actionOpenLedgerTab = new();
    public ActionControl.ActionOpenLedgerSelectPage actionOpenLedgerSelectPage = new();
    public ActionControl.ActionRunDialogue actionRunDialogue = new();
    public ActionControl.AfterFlipBehaviour afterFlipBehaviour = new();
    public ActionControl.ActionOnSwitchScene actionOnSwitchScene = new();
    public ActionControl.FragAction<LedgerMovement> fragActionLedgerManager = new();

    //=============================================================================================================================================================
    public static Action<DialogueManager> AFTERDIALOGUE;
    public static Action<LedgerManager> PRESSTAB_LEDGER;
    public static Action<LedgerManager> PRESSRETURN_LEDGER;
    public static Action<LedgerMovement> AFTERPAGEFLIP_LEDGER;
    public static Action<LedgerManager> AFTERPAGEFLIPFURTHESTLEFT_LEDGER;
    public static Action<SceneManager> AFTERENTERINGSCENE; //runs after onenter for the scene state you are in.

    public bool DoesActionExist<T>(Action<T> action, Action<T> actionSub)
    {
        if (action == null)
        {
            return false;
        }
        if (action.GetInvocationList().Contains(actionSub))
            {
                return true;
            }
        return false;
    }
   
    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager mm) => { mm.actionController = this; });
        PlayerData.INSTANCE?.playerRaycast.subjectClue.AddObserver(this);
    }
   

    public override void m_Start()
    {
        //TODO change below.
        if (!DoesActionExist(AFTERPAGEFLIP_LEDGER, afterFlipBehaviour.pointActionLedgerMovement))
        {
            Debug.Log("action set for ledger");
            AFTERPAGEFLIP_LEDGER += afterFlipBehaviour.pointActionLedgerMovement;
        }
       
        if (!DoesActionExist(AFTERENTERINGSCENE, actionOnSwitchScene.switchSceneDefault))
        {
            Debug.Log("action set for scene");
            AFTERENTERINGSCENE += actionOnSwitchScene.switchSceneDefault;
        }
        base.m_Start();
    }


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

   

    public void OnNotify(PlayerActions actionData, ClueMono myObject)
    {
        if (actionData == PlayerActions.onOmitRayClue)
        {
            /*
            PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabStartCutscene;
            PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabOpenLedger;
            PRESSTAB_LEDGER(LedgerManager.INSTANCE);   
            */  
        }
    }
}
