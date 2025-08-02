using System.Collections;
using System.Collections.Generic;
using InstGameObject;
using UnityEngine;

public class InstGameObjectManager : MonoBehaviour, IExecution
{
    [SerializeField]
    private InstGameObject.UIGameobjectsScriptableObject uIGameobjectsScriptableObject;
    [SerializeField]
    private InstGameObject.CameraGameObjectScriptableObject cameraGameobjectScriptableObject;

    public void m_Awake()
    {
        InstLedgerCam();
        InstUI();
       

    }

    public void m_GameExecute()
    {

    }

    public void m_OnEnable()
    {

    }

    private void InstUI()
    {
        var uI = GameObject.FindGameObjectWithTag("UI");

        var ledgerUI = Instantiate(uIGameobjectsScriptableObject?.LedgerUI, uI?.transform);
        ledgerUI.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("LedgerCams").GetComponentsInChildren<Camera>()[0];
        
        Instantiate(uIGameobjectsScriptableObject?.UICanvas, uI?.transform);
    }
    
    private void InstLedgerCam()
    {
        var ledgerCams = GameObject.FindGameObjectWithTag("LedgerCams");
        Instantiate(cameraGameobjectScriptableObject.ledgerCamNoise, ledgerCams?.transform);
        Instantiate(cameraGameobjectScriptableObject.ledgerCam_NoNoise, ledgerCams?.transform);
    }
}

