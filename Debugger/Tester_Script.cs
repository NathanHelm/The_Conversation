using UnityEngine;
using System.Collections;
using Codice.Client.BaseCommands.BranchExplorer;

public class Tester_Script : MonoBehaviour
{
    [SerializeField]
    Texture2D[] texture;

    private void Start()
    {
        UI.LedgerUIManager.INSTANCE.m_Start();
        LedgerManager.INSTANCE.m_Start();
        LedgerImageManager.INSTANCE.m_Start();
        for(int i = 0; i < 15; i++)
        {
            LedgerImageManager.INSTANCE.AddRayInfoToLedgerImage(i + 1, "description of image " + i + ". ", new int[] {1, 2, 3}, texture[i], new int[] {1} );
        }
        
       
       
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

