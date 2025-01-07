using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public class LedgerManager : StaticInstance<LedgerManager>
{
    public static SystemActionCall<LedgerManager> onStartLedgerData = new SystemActionCall<LedgerManager>();
    public static SystemActionCall<LedgerManager> onShowLedgerImages = new SystemActionCall<LedgerManager>();

    public List<LedgerImage> ledgerImages { get; set; }

    public GameObject bookObj { get; set; }




    public override void m_Start()
    {
        MManager.onStartManagersAction.AddAction((MManager m) => { m.ledgerManager = this; });
        bookObj = GameObject.FindGameObjectWithTag("Book");
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

