using UnityEngine;
using System.Collections;
using Codice.Client.BaseCommands.BranchExplorer;

public class Tester_Script : MonoBehaviour
{
    [SerializeField]
    Texture2D[] texture;
    [SerializeField]
    Texture2D[] overlayTex;
    [SerializeField]
    bool isReplace = false;

    private void Start()
    {
        
        UI.LedgerUIManager.INSTANCE.m_Start();
        LedgerManager.INSTANCE.m_Start();
        LedgerImageManager.INSTANCE.m_Start();
        AnimationManager.INSTANCE.m_Start();
       
        LedgerMovement.INSTANCE.m_Start();
        HandAnimations.INSTANCE.m_Start();
        StateManager.INSTANCE.m_Start();
        LedgerMovement.INSTANCE.EnableHand();
        

        if(texture.Length == 0)
        {
            Debug.LogError("add images for this tester script to show images :)");
        }
        for(int i = 0; i < texture.Length; i++)
        {
            LedgerImageManager.INSTANCE.AddRayInfoToLedgerImage("description " + i + ".", 0, ClueMono.clueQuestionID, texture[i], overlayTex, null, 0);
        }
        
       
       
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
       //     LedgerManager.INSTANCE.OpenLedger();
        }
      
        //LedgerManager.INSTANCE.UseLedgerState();
        //LedgerManager.INSTANCE.EnableLedger();
        //LedgerManager.INSTANCE.MovePages();
        if(Input.GetKeyDown(KeyCode.R))
        {
            LedgerManager.INSTANCE.WriteToPageInLedger();
        }

        /*
        if(isReplace)
        {
            LedgerImageManager.INSTANCE.temporaryImage = new LedgerImage(null, 0, ClueMono.clueQuestionID, texture[1], overlayTex, null, 0) ;
            LedgerManager.INSTANCE.ReplacePage();
        }

        LedgerManager.INSTANCE.MovePages();
        */
       // LedgerManager.INSTANCE.ReplacePage();
     
    }
    
}

