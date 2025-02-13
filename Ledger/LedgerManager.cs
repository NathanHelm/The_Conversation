using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;

public class LedgerManager : StaticInstance<LedgerManager>
{
    public static SystemActionCall<LedgerManager> onStartLedgerData = new SystemActionCall<LedgerManager>();
    public static SystemActionCall<LedgerManager> onShowLedgerImages = new SystemActionCall<LedgerManager>();

    private bool isLedgerEnabled;

    int index = 0;

    public readonly int ledgerLength = 10;

    public List<LedgerImage> ledgerImages { get; set; }

  

    public override void OnEnable()
    {

        MManager.onStartManagersAction?.AddAction((MManager m) => { m.ledgerManager = this; });

        base.OnEnable();
    }


    public override void m_Start()
    {
        CreateLedger();
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
            --index;
            // index = Mathf.Clamp(index, 0, ledgerLength - 1);
            Debug.Log("index " + index);
            if (index < 0)
            {
                index = Mathf.Clamp(index, 0, ledgerLength - 1);
                return;
            }
           
            index = Mathf.Clamp(index, 0, ledgerLength - 1);
            UI.LedgerUIManager.INSTANCE.FlipPageLeft(index);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ++index;
            //  index = Mathf.Clamp(index, 0, ledgerLength - 1);
            Debug.Log("index " + index);
            if (index > ledgerLength - 1)
            {
                index = Mathf.Clamp(index, 0, ledgerLength - 1);
                return;
            }
            index = Mathf.Clamp(index, 0, ledgerLength - 1);
            UI.LedgerUIManager.INSTANCE.FlipPageRight(index);
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
            throw new Exception("ledger images are null");
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
        UI.LedgerUIManager.INSTANCE.ReplacePageSprite(LedgerData.INSTANCE.ledgerImages.Count - 1, ledgerImageSprite);
        
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

