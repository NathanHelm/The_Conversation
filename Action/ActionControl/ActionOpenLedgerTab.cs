using System.Collections;
using System.Collections.Generic;
using System;
using Data;
using UnityEngine;
using log4net;
namespace ActionControl
{
    public class ActionOpenLedgerTab
    {
        //wait...this is just a statemachine for a specific action
        public Action<LedgerManager> pressTabEnterPreviousScene = lm =>
        {
            Debug.Log("err-1");
            GameEventManager.INSTANCE.OnEvent(SceneManager.INSTANCE.enumSceneNameToSceneState[InterviewData.PREVIOUSSCENE]);
        };
        public Action<LedgerManager> pressTabEnterInterviewScene = lm =>
        {
              Debug.Log("err-2");
             GameEventManager.INSTANCE.OnEvent(typeof(InterviewSceneState));
        };
        public Action<LedgerManager> pressTabDisableLedger = lm =>
        {
              Debug.Log("err-3");
            string temp = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
            GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));
        };
        public Action<LedgerManager> pressTabOpenLedger = lm =>
        {
              Debug.Log("err-4");
            GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState)); //hands are automatically enabled on open ledger state  
        };
        public Action<LedgerManager> pressTabStopDialogue = lm =>
        {
              Debug.Log("err-5");
            GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));
        };
        public Action<LedgerManager> pressTabStartCutscene = lm =>
        {
              Debug.Log("err-6");
            GameEventManager.INSTANCE.OnEvent(typeof(PlayCutsceneState));
        };
        public Action<LedgerManager> pressTabStopCutscene = lm =>
        {
              Debug.Log("err-7");
            GameEventManager.INSTANCE.OnEvent(typeof(StopCutsceneState));
        };
        

    }
}