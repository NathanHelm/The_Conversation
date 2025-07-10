using System;
using System.Collections;
using System.Collections.Generic;
using Codice.LogWrapper;
using UnityEngine;

public class Action_Test : MonoBehaviour
{
    Action<int> action;
    public void Test1(int a)
    {
        Debug.Log("Test: 1");

    }
    public void Test2(int b)
    {
          Debug.Log("Test: 2");
    }

    public void Start()
    {
        action += Test1;
        action += Test2;
       action = (int i) => { };
        action(5);

    }
}
