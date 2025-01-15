using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;

public class LedgerManager : StaticInstance<LedgerManager>
{
    public static SystemActionCall<LedgerManager> onStartLedgerData = new SystemActionCall<LedgerManager>();
    public static SystemActionCall<LedgerManager> onShowLedgerImages = new SystemActionCall<LedgerManager>();
    public readonly int ledgerLength = 5;
    public List<LedgerImage> ledgerImages { get; set; }

    public GameObject bookObj { get; set; }

    public override void OnEnable()
    {

        MManager.onStartManagersAction.AddAction((MManager m) => { m.ledgerManager = this; });
        base.OnEnable();
    }


    public override void m_Start()
    {
        if (GameObject.FindGameObjectWithTag("Book") != null)
        {
            bookObj = GameObject.FindGameObjectWithTag("Book");
        }
        base.m_Start();
        onStartLedgerData.RunAction(this);
    }

    //todo create a ledger state machine...

    public void OpenBook() //enables book gameobject 
    {
        GameObject nbookObj = GetBookObj();
        UIManager.INSTANCE.EnableUIObject(ref nbookObj);
    }
    public void CloseBook()
    {
        GameObject nbookObj = GetBookObj();
        UIManager.INSTANCE.DisableUIObject(ref nbookObj);
    }


    public GameObject GetBookObj()
    {
        return bookObj;
    }
    public void AddRayInfoToLedgerImage(int bodyId, string dialogueDescription, (int, int) characterNameQuestion, int[] customQuestions) //converts ray information to ledger image object
    {
        LedgerImage ledgerImage = new (dialogueDescription, characterNameQuestion, customQuestions, bodyId);
        if(LedgerData.INSTANCE.ledgerImages.Count > ledgerLength)
        {
            //todo apply restart animation.
            ledgerImages = new List<LedgerImage>();
            //todo destroy ledgerimages in persistent data
        }
        LedgerData.INSTANCE.ledgerImages.Add(ledgerImage);
    }

    public void ShowLedgerImages()
    {
        onShowLedgerImages.RunAction(this);
        if(ledgerImages == null)
        {
            throw new Exception("ledger images are null");
        }
        for(int i = 0; i < ledgerImages.Count; i ++)
        {
            //to do add animation
            Debug.Log("added image");
        }

       
    }
    
   


}

