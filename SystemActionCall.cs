using UnityEngine;
using System.Collections;
using System;

public class SystemActionCall<T>
{
    //adds actions between static instance systems  
    public static Action<T> actionSetup { get; set; }
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
        try
        {
            initialAction -= action;
            actionSetup -= action;
        }
        catch(Exception e)
        {
           Debug.LogError(e); 
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



