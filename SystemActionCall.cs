using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Diagnostics;

public class SystemActionCall<T>
{
    //adds actions between static instance systems  
    public Action<T> actionSetup { get; set; }
    private Action<T> initialAction;


    public void AddPriorityAction(Action<T> action)
    {
        initialAction += action;
    }
    public void AddAction(Action<T> action)
    { 
               
        actionSetup += action;
    }
    public bool IsThereAction(Action<T> action)
    {
        if(actionSetup.GetInvocationList().Contains(action) ||
         initialAction.GetInvocationList().Contains(initialAction))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
  

    public void RemoveAction(Action<T> action)
    {
        if (actionSetup != null)
        {
            if (actionSetup.GetInvocationList().Contains(action))
            {
                actionSetup -= action;
            }
            else
            {
                UnityEngine.Debug.LogError("action cannot be found");
                UnityEngine.Debug.Log("invocation size" + actionSetup.GetInvocationList());
            }
        }
        if (initialAction != null)
        {
            if (initialAction.GetInvocationList().Contains(action))
            {
                initialAction -= action;
            }
        }
    }
    public void RemoveAllActions()
    {
        initialAction = null;
        actionSetup = null;
    }
    public void RunAction(T actionMethodParameter)
    {            
        initialAction?.Invoke(actionMethodParameter);
        actionSetup?.Invoke(actionMethodParameter);
    }

}



