using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;
using UI;
using System.Threading.Tasks;
using Unity.VisualScripting.YamlDotNet.Serialization;

public class LedgerManager : StaticInstance<LedgerManager>
{
    public static SystemActionCall<LedgerManager> onStartLedgerData = new SystemActionCall<LedgerManager>();
    public static SystemActionCall<LedgerManager> onShowLedgerImages = new SystemActionCall<LedgerManager>();

    private bool isLedgerEnabled;

    int index = 0;

    public readonly int ledgerLength = 3;

    public List<LedgerImage> ledgerImages { get; set; }

    int previousPage;



  

    public override void OnEnable()
    {

        MManager.onStartManagersAction?.AddAction((MManager m) => { m.ledgerManager = this; });

        base.OnEnable();
    }


    public override void m_Start()
    {
        Debug.Log("NOTE: ledger has not been made");
        //CreateLedger();
        base.m_Start();
        onStartLedgerData.RunAction(this);
    }

    public void UseLedger()
    {
        //Pressing tab opens the ledger
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            PlayUICutscene();
        }

    }

    public void MovePages()
    {
     
        if(Input.GetKeyDown(KeyCode.A))
        {
          
           if(previousPage > index) //i want to previous page
           {
             index = previousPage;
             UI.LedgerUIManager.INSTANCE.FlipPageLeft(previousPage);
             return;
           }

            UI.LedgerUIManager.INSTANCE.FlipPageLeft(index);

            previousPage = index;

            --index;

            index = Mathf.Clamp(index, 0, ledgerLength - 1);
            Debug.Log("index " + index);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
           if(previousPage < index) //i want to previous page
           {
             index = previousPage;
             UI.LedgerUIManager.INSTANCE.FlipPageRight(previousPage);
             return;
           }
          
        
            UI.LedgerUIManager.INSTANCE.FlipPageRight(index);

            previousPage = index;

             ++index;

            index = Mathf.Clamp(index, 0, ledgerLength - 1);
            Debug.Log("index " + index);
            
           
      
        }
    }
    public void EnableLedger()
    {
        UI.LedgerUIManager.INSTANCE.OpenBook();
    }
    public void DisableLedger()
    {
        UI.LedgerUIManager.INSTANCE.CloseBook();
    }

    //todo create a ledger state machine...
    public void CreateLedger()
    {
        onShowLedgerImages.RunAction(this);

        if (ledgerImages == null)
        {
           Debug.LogError("ledger images are null");
           return;
        }

        for (int i = 0; i < ledgerLength; i++)
        {
            //adds page
            /*
            if(i == 0 || i == ledgerLength - 1)
            {
                UI.LedgerUIManager.INSTANCE.add
                continue;
            }
            */
            UI.LedgerUIManager.INSTANCE.AddPage(i, ledgerLength - 1);
        }
    }
    

    public void AddRayInfoToLedgerImage(int bodyId, string dialogueDescription, (int, int) characterIdQuestionId, int[] customQuestions, Sprite ledgerImageSprite) //converts ray information to ledger image object
    {
        LedgerImage ledgerImage = new(dialogueDescription, characterIdQuestionId, customQuestions, bodyId, ledgerImageSprite);
        if (LedgerData.INSTANCE.ledgerImages.Count > ledgerLength)
        {
            //todo apply restart animation.
            throw new Exception("ledger image count greater the ledger page cap.\n give last page a time limit ");
            //todo destroy ledgerimages in persistent data
        }
        LedgerData.INSTANCE.ledgerImages.Add(ledgerImage);

        //TODO --> interesting.... I want to change the page's material BASED on the image drawn. (some function called image drawn sprite)
        //an idea: make drawing sprite THEN overlay it.
        //another idea = set drawing sprite as a texture and use some lerping function with the page. 

       // UI.LedgerUIManager.INSTANCE.ReplacePageSprite(LedgerData.INSTANCE.ledgerImages.Count - 1, ledgerImageSprite);
        
    }


    private void PlayUICutscene()
    {
        //add this as ledger state
        isLedgerEnabled = true;
        /*
        CutsceneManager.INSTANCE.SetSnapShot( new (string, Type)[] { new("LedgerState",typeof(ActiveLedgerState)),  });
        CutsceneManager.INSTANCE.SetCutSceneConditions(new (Action, bool)[] { ( () => { Debug.Log("cutscene with condition is active..."); }, isLedgerEnabled)});
        CutsceneManager.INSTANCE.PlayCutscene();
        */
        GameEventManager.INSTANCE.OnEvent(typeof(ActiveLedgerState));

    }


}

