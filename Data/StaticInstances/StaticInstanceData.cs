using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInstanceData<T> : StaticInstance<T> where T : MonoBehaviour, IExecution
{
    public override void OnDisable()
    {
        Debug.Log("This is a disable object");
    }
    public override void m_OnEnable()
    {
        base.m_OnEnable();
    }
    public override void m_Awake()
    {
        base.m_Awake();
    }
    public override void m_GameExecute()
    {
        base.m_GameExecute();
    }
    
}