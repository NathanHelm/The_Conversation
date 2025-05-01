using UnityEngine;
using System.Collections;
using System;
using System.Linq;

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
    public void RemoveAction(Action<T> action)
    {
        if(actionSetup != null)
        {
        if(actionSetup.GetInvocationList().Contains(action))
        {
            actionSetup -= action;
        }
        }
        if(initialAction != null)
        {
        if(initialAction.GetInvocationList().Contains(action))
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



