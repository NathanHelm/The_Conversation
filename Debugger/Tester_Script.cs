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
       if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("add a page");
            //LedgerManager.INSTANCE.AddRayInfoToLedgerImage(1, "descriptionExample", (1, 2), new int[] { 2, 4 }, null, 1);

        }
    }
    
}

