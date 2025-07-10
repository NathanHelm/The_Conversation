using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TestTools;

public class GameExecutionManager : MonoBehaviour
{
    public void OnEnable()
    {
        Execute();
    }

    public void Execute()
    {
        IExecution[] allMonoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IExecution>().ToArray();

        foreach (var single in allMonoBehaviours)
        {
            single.m_Awake();
        }
        foreach (var single in allMonoBehaviours)
        {
            single.m_OnEnable();
        }
        foreach (var single in allMonoBehaviours)
        {
            single.m_GameExecute();
        }
     
    }

}