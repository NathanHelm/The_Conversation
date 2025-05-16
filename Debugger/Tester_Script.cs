using UnityEngine;
using System.Collections;
using Codice.Client.BaseCommands.BranchExplorer;
using Data;

public class Tester_Script : MonoBehaviour
{
    [SerializeField]
    Texture2D[] texture;
    [SerializeField]
    Texture2D[] overlayTex;
    [SerializeField]
    bool isReplace = false;

    
    public static LedgerImage temporaryImageTest;

    private void Start()
    {
       /* 
        UI.LedgerUIManager.INSTANCE.m_Start();
        LedgerManager.INSTANCE.m_Start();
        LedgerImageManager.INSTANCE.m_Start();
        AnimationManager.INSTANCE.m_Start();
       
        LedgerMovement.INSTANCE.m_Start();
        HandAnimations.INSTANCE.m_Start();
        StateManager.INSTANCE.m_Start();
        PageAnimations.INSTANCE.m_Start();
        DrawingManager.INSTANCE.m_Start();
       */

      //  temporaryImageTest = new LedgerImage("description " + 0 + ".", 0, ClueMono.clueQuestionID, texture[0], overlayTex, null, 0);
        if(texture.Length == 0)
        {
            Debug.LogError("add images for this tester script to show images :)");
        }
        for(int i = 0; i < texture.Length; i++)
        {
            LedgerImageManager.INSTANCE.AddRayInfoToLedgerImage("description " + i + ".", 0, 0, texture[i], overlayTex, 0);
    
        }
        
        
       
       
    }
    private void Update()
    {
 
      
        //LedgerManager.INSTANCE.UseLedgerState();
        //LedgerManager.INSTANCE.EnableLedger();
        //LedgerManager.INSTANCE.MovePages();
        if(Input.GetKeyDown(KeyCode.Return))
        {
          // GameEventManager.INSTANCE.OnEvent(typeof(WriteToPageLedgerState));
         // ImitateOmitRay();
          for(int i = 0; i < texture.Length; i++)
        {
          //  LedgerImageManager.INSTANCE.AddRayInfoToLedgerImage("description " + i + ".", 0, ClueMono.clueQuestionID, texture[i], overlayTex, null, 0);
           
        }
        // LedgerManager.INSTANCE.WriteToPageInLedger();
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            GameEventManager.INSTANCE.OnEvent(typeof(ReplaceLedgerState));
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
    private void ImitateOmitRay()
    {
        ClueMono clueMonoInRay = FindObjectOfType<ClueMono>().GetComponent<ClueMono>();
        Texture IMAGECREATORTEXTURE = DrawingManager.INSTANCE.TakeScreenShot();
        LedgerImageManager.INSTANCE.AddRayInfoToLedgerImage(clueMonoInRay.imageDescription, clueMonoInRay.questionID, clueMonoInRay.clueQuestionID , IMAGECREATORTEXTURE,clueMonoInRay.ledgerOverlays, ClueMono.clueBodyID); //adding 'hit data information to ledger manager'
        GameEventManager.INSTANCE.OnEvent(typeof(WriteToPageLedgerState));
        DrawingManager.INSTANCE.RunDrawingPPEffect();
       // GameEventManager.I
    }
    
}

