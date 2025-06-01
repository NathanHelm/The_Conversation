using System.Diagnostics;
using Data;
using UnityEditor.IMGUI.Controls;

public class VetHouseSceneState : SceneState{

    public override void OnEnter(SceneData data) //on enter will run from the previous scene transition
    {
        UnityEngine.Debug.Log("on vet house scene!");

        GameEventManager.INSTANCE.OnEvent(typeof(EndConversationState));


        //when we select the page when its open... 
        LedgerManager.onSelectPage.AddAction(LedgerData.INSTANCE.runClueDialogueOnSelectPage);
      
       


    }
    public override void OnExit(SceneData data)
    {
        MManager.onStartManagersAction.RemoveAllActions();

        LedgerManager.onSelectPage.RemoveAction(LedgerData.INSTANCE.runClueDialogueOnSelectPage);

        //1 -- determine what needs to be saved on exit (if anything...)
        if (data.nextScene == SceneNames.InterviewScene)
        {
            //save on interview scene 
            // 1) player position 
            // 2)character data
            SavePersistenceManager.INSTANCE.SaveInterfaceData(InterviewData.INSTANCE);
            SavePersistenceManager.INSTANCE.SaveInterfaceData(CharacterManager.INSTANCE);
            SavePersistenceManager.INSTANCE.SaveInterfaceData(LedgerImageManager.INSTANCE);
            
        }
        else
        {

            SavePersistenceManager.INSTANCE.Save(); //save every implemented object in any other scenario.
        }
       
    }

    
}