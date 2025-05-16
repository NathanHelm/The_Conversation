using System.Runtime.InteropServices;
using Data;
using UnityEngine;
public class PlayerQuestionConversationMemoryState: ConversationState{
    //1- open ledger
    //2- search page
    //3- press return on page
    //4- with page id, get the question id
    //5-

    public override void OnEnter(DialogueData data)
    {
        LedgerManager.INSTANCE.EnableLedger();
    }
    public override void OnUpdate(DialogueData data)
    {
        LedgerManager.INSTANCE.MovePages();

        if(InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return))
        {
          
          // PlayerData.INSTANCE.currentQuestionID = LedgerManager.INSTANCE.GetQuestionsIDFromPage()[0];
           Debug.Log("Got question from page image");

          // GameEventManager.INSTANCE.OnEvent();
        }

    }


}