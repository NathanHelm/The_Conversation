using Data;

public class MemorySceneState : SceneState
{
    public override void OnEnter(SceneData data)
    {
        CharacterManager.INSTANCE?.Load();

        GameEventManager.INSTANCE.OnEvent(typeof(DisableHandState));
        GameEventManager.INSTANCE.OnEvent(typeof(DisableLedgerState));
        GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));



        ActionController.PRESSRETURN_LEDGER += ActionController.INSTANCE.actionOpenLedgerSelectPage.runClueDialogueOnSelectPage;


        LedgerImageManager.INSTANCE?.Load();
        QuestionResponseManager.INSTANCE?.Load();

        MemorySpawnerManager.INSTANCE.Load(); //spawns stage objects... 
    }
    public override void OnExit(SceneData data)
    {
        MManager.INSTANCE.onStartManagersAction.RemoveAllActions();
        ActionController.PRESSRETURN_LEDGER -= ActionController.INSTANCE.actionOpenLedgerSelectPage.runClueDialogueOnSelectPage;
        ActionController.PRESSTAB_LEDGER = lm => { };
        //SavePersistenceManager.INSTANCE.Save(); //save every implemented object in any other scenario.
        base.OnExit(data);
    }
}