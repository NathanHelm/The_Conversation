using System.Diagnostics;
using Data;
using UnityEditor.IMGUI.Controls;

public class VetHouseSceneState : SceneState{

    public override void OnEnter(SceneData data) //on enter will run from the previous scene transition
    {
        UnityEngine.Debug.Log("on vet house scene!");


        //runs when ledger has been created...
        /*
        LedgerManager.INSTANCE.onAfterCreateLedger.AddAction(
             lm => {    LedgerImageManager.INSTANCE?.Load()
               ; }
        );
        */
        GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));
        GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
        GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));

        LedgerImageManager.INSTANCE?.Load();

        QuestionResponseManager.INSTANCE?.Load();


        //when we select the page when its open... 
        //LedgerManager.INSTANCE.onSelectPage.AddAction(ActionController.INSTANCE.actionOpenLedgerSelectPage.runClueDialogueOnSelectPage);

        ActionController.PRESSRETURN_LEDGER += ActionController.INSTANCE.actionOpenLedgerSelectPage.runClueDialogueOnSelectPage;
       


    }
    public override void OnExit(SceneData data)
    {
        //LedgerManager.INSTANCE.onAfterCreateLedger.RemoveAllActions();
        MManager.INSTANCE.onStartManagersAction.RemoveAllActions();

        ActionController.PRESSRETURN_LEDGER -= ActionController.INSTANCE.actionOpenLedgerSelectPage.runClueDialogueOnSelectPage;
       
        ActionController.PRESSTAB_LEDGER = lm => { };

        //1 -- determine what needs to be saved on exit (if anything...)
        if (data.nextScene == SceneNames.InterviewScene)
        {
            //save on interview scene 
            // 1) player position 
            // 2)character data
            SavePersistenceManager.INSTANCE.SaveInterfaceData(SpawnData.INSTANCE);
            SavePersistenceManager.INSTANCE.SaveInterfaceData(InterviewData.INSTANCE);
            SavePersistenceManager.INSTANCE.SaveInterfaceData(CharacterManager.INSTANCE);
            SavePersistenceManager.INSTANCE.SaveInterfaceData(LedgerImageManager.INSTANCE);

        }
        else
        {
            //  SavePersistenceManager.INSTANCE.Save(); //save every implemented object in any other scenario.
        }
       
    }

    
}