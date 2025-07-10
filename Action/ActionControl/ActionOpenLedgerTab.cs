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
            GameEventManager.INSTANCE.OnEvent(SceneManager.INSTANCE.enumSceneNameToSceneState[InterviewData.INSTANCE.previousScene]);
        };
        public Action<LedgerManager> pressTabEnterInterviewScene = lm =>
        {
             GameEventManager.INSTANCE.OnEvent(typeof(InterviewSceneState));
        };
        public Action<LedgerManager> pressTabDisableLedger = lm =>
        {
            string temp = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
            GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));
        };
        public Action<LedgerManager> pressTabOpenLedger = lm =>
        {
            Debug.Log("a strange open ledger");
            GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState)); //hands are automatically enabled on open ledger state  
        };
        public Action<LedgerManager> pressTabStopDialogue = lm =>
        {
            GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));
        };
        public Action<LedgerManager> pressTabStartCutscene = lm =>
        {
            GameEventManager.INSTANCE.OnEvent(typeof(PlayCutsceneState));
            Debug.Log("a strange occurence here!");
        };
        public Action<LedgerManager> pressTabStopCutscene = lm =>
        {
            GameEventManager.INSTANCE.OnEvent(typeof(StopCutsceneState));
        };
        

    }
}