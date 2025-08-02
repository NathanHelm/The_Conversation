using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Data;
namespace ActionControl
{
    public class ActionOpenLedgerSelectPage
    {
        public Action<LedgerManager> runDialogOnSelectPageInterviewScene = lm =>
        {
            int pageObjectsIndex = Data.LedgerData.INSTANCE.pageIndex;
            if (!LedgerImageManager.INSTANCE.IsIndexInLedgerImageListRange(pageObjectsIndex))
            {
                Debug.LogError("ledger image is not in range!");
                return;
            }
            int currentClueId = LedgerImageManager.INSTANCE.GetQuestionIDFromPage(pageObjectsIndex);
            //"LOG: getting clue id from page object " + currentClueId);
            DialogueData.INSTANCE.currentQuestionID = currentClueId;
            GameEventManager.INSTANCE.OnEvent(typeof(ClueConversationState));
        };
        public Action<LedgerManager> runClueDialogueOnSelectPage = lm =>
        {
            int pageObjectsIndex = Data.LedgerData.INSTANCE.pageIndex;
            if (LedgerImageManager.INSTANCE.IsIndexInLedgerImageListRange(pageObjectsIndex))
            {
                int currentClueBodyId = LedgerImageManager.INSTANCE.GetClueBodyIDFromPage(pageObjectsIndex);
                int currentCludId = LedgerImageManager.INSTANCE.GetClueQuestionIDFromPage(pageObjectsIndex);
                DialogueData.INSTANCE.currentCharacterID = currentClueBodyId;
                DialogueData.INSTANCE.currentQuestionID = currentCludId;
                GameEventManager.INSTANCE.OnEvent(typeof(ClueConversationState));
                GameEventManager.INSTANCE.OnEvent(typeof(InspectClueLedgerState));
            }
        };

    }
}