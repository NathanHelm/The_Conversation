using UnityEngine;

public class ClueCameraLook : MonoBehaviour, IExecution
{
    public Transform clueTrans { get; set; }
    public void m_Awake()
    {
        //throw new System.NotImplementedException();
    }

    public void m_GameExecute()
    {

       //throw new System.NotImplementedException();
    }

    public void m_OnEnable()
    {
      
    }
    public void Update()
    {
        transform?.LookAt(clueTrans);
    }
}