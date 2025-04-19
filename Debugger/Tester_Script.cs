using UnityEngine;
using System.Collections;

public class Tester_Script : MonoBehaviour
{

    private void Start()
    {
        UI.LedgerUIManager.INSTANCE.m_Start();
        LedgerManager.INSTANCE.m_Start();

        
       
       
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            LedgerManager.INSTANCE.OpenLedger();
        }
        //LedgerManager.INSTANCE.UseLedgerState();
        //LedgerManager.INSTANCE.EnableLedger();
        //LedgerManager.INSTANCE.MovePages();
        if(Input.GetKeyDown(KeyCode.R))
        {
            LedgerManager.INSTANCE.WriteToPageInLedger();
        }

        LedgerManager.INSTANCE.MovePages();
       // LedgerManager.INSTANCE.ReplacePage();
     
    }
    
}

