using System.Diagnostics;
using Data;
using UnityEditor.IMGUI.Controls;

public class VetHouseSceneState : SceneState{

    public override void OnEnter(SceneData data) //on enter will run from the previous scene transition
    {
        UnityEngine.Debug.Log("on vet house scene!");

        LedgerManager.onSelectPage.AddAction(LedgerData.INSTANCE.runClueDialogueOnSelectPage);
       //1 -- change state
       //not needed here
       
       //2 -- determine what gets loaded (and what doesn't!)


    }
    public override void OnExit(SceneData data)
    {
       //1 -- determine what needs to be saved on exit (if anything...)
        if(data.nextScene == SceneNames.InterviewScene)
        {
            //save on interview scene 1) player position 2) 
            InterviewData.INSTANCE.Save();

            
        }
        else
        {
            SavePersistenceManager.INSTANCE.Save(); //save every implemented object in any other scenario.
        }
       
    }

    
}