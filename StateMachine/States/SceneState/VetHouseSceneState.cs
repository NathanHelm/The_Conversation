using Data;

public class VetHouseSceneState : SceneState
{
    public override void OnEnter(SceneData data)
    {
       
        SceneData.CURRENTSCENE = SceneNames.VetHouseScene;
        GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));
        GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
        GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));
        UnityEngine.Debug.Log("ledger image load");
        LedgerImageManager.INSTANCE?.Load();
        QuestionResponseManager.INSTANCE?.Load();
        MemoryManager.INSTANCE?.Load();
        ActionController.PRESSRETURN_LEDGER += ActionController.INSTANCE.actionOpenLedgerSelectPage.runClueDialogueOnSelectPage;
    }
    public override void OnExit(SceneData data)
    {
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
            SavePersistenceManager.INSTANCE.SaveInterfaceData(ClueCameraManager.INSTANCE);
            SavePersistenceManager.INSTANCE.SaveInterfaceData(LedgerImageManager.INSTANCE);

            SavePersistenceManager.INSTANCE.SaveInterfaceData(MemoryManager.INSTANCE); //save memory dictionary
          
        }
        else
        {
            //  SavePersistenceManager.INSTANCE.Save(); //save every implemented object in any other scenario.
        }
        base.OnExit(data);
    }
}