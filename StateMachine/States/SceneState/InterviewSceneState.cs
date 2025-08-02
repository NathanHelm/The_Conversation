using System.Diagnostics;
using Data;

public class InterviewSceneState : SceneState{
    public override void OnEnter(SceneData data)
    {

        //1 -- determine what get loaded (and what doesn't!)
        // SavePersistenceManager.INSTANCE.Load();
        //  DialogueManager.INSTANCE
        //2 -- change state (when you press return after conversation


        InterviewData.INSTANCE?.Load(); //load dialogue data with proper data...
        CharacterManager.INSTANCE?.Load();
        ActionController.PRESSTAB_LEDGER = lm => { };
        GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState));

        ActionController.PRESSTAB_LEDGER += ActionController.INSTANCE.actionOpenLedgerTab.pressTabEnterPreviousScene;
        ActionController.PRESSRETURN_LEDGER += ActionController.INSTANCE.actionOpenLedgerSelectPage.runDialogOnSelectPageInterviewScene;
       
        LedgerImageManager.INSTANCE?.Load();
        QuestionResponseManager.INSTANCE?.Load();
        //after dialogue, you are take 
        //  DialogueManager.onEndDialogue.AddAction(DialogueData.INSTANCE.runEndClueInspection;);
    }
    public override void OnExit(SceneData data)
    {
        MManager.INSTANCE.onStartManagersAction.RemoveAllActions();

        SavePersistenceManager.INSTANCE.SaveInterfaceData(MemoryManager.INSTANCE);

        ActionController.PRESSRETURN_LEDGER -= ActionController.INSTANCE.actionOpenLedgerSelectPage.runDialogOnSelectPageInterviewScene;
        ActionController.PRESSTAB_LEDGER = lm => { UnityEngine.Debug.Log("exit interview log!"); };
        base.OnExit(data);
    }
}