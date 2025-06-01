using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameEventManager : StaticInstance<GameEventManager>
{
	public Dictionary<string, Action> onStateNameToAction = new Dictionary<string, Action>(); //gets a event name (OnTransitionTo2d for example) and adds a function to it. 

    private Dictionary<string, Action> previousAction = new Dictionary<string, Action>(); //gets a event name (OnTransitionTo2d for example) and adds a function to it. 

    //public Dictionary<string, Func<object, object>> onIDtoReturnFuncPara = new Dictionary<string, Func<object, object>>();

    // public Dictionary<string, Func<object>> onIDtoReturnFunc = new Dictionary<string, Func<object>>();

    public void ReplaceEvent(string name, Action action)
    {
        if (!onStateNameToAction.ContainsKey(name))
        {
            throw new KeyNotFoundException("key " + name + "not found in the dictionary.");
           
        }
        if (onStateNameToAction[name].Equals(action))
        {
            throw new Exception("action is the same");
        }
        SavePrevEvent(name, action);
        onStateNameToAction[name] = action;
    }
    public void ReplaceWithPrevEvent(string name)
    {
        if (!previousAction.ContainsKey(name))
        {
            throw new KeyNotFoundException("key " + name + "not found in the dictionary.");
        }
        onStateNameToAction[name] = previousAction[name]; //replaces current event action with stored action.
    }
    private void SavePrevEvent(string name, Action action)
    {
        previousAction.Add(name, action);
    }


	public void OnEvent(Type stateName) //to run the event, simply use this function on a desired object.
	{
		if (!onStateNameToAction.ContainsKey(stateName.ToString()))
		{
            Debug.LogError("state " + stateName.ToString() + " not found. \nAdd state to statemanager.");
			return;
		}
		onStateNameToAction[stateName.ToString()]();
	}
    public void OnEvent(string stateName)
    {
        if (!onStateNameToAction.ContainsKey(stateName.ToString()))
        {
            Debug.LogError("state " + stateName.ToString() + "not found. \nAdd state to statemanager.");
            return;
        }
        onStateNameToAction[stateName]();
    }


    public T3 OnEventFunc<T1, T2, T3>(string funcKey, T1 para, T2 para2)
    {
        string[] keys = GameEventGeneric<T1, T2, T3>.dict3.Keys.ToArray();

        if (!GameEventGeneric<T1, T2, T3>.dict3.ContainsKey(funcKey))
        {
            for(int i = 0; i < keys.Length; i++)
            {
                Debug.Log(keys[i]);
            }
            throw new KeyNotFoundException(funcKey + "not found ");
            
        }
        return GameEventGeneric<T1, T2, T3>.dict3[funcKey](para, para2); //sus
    }
    public T2 OnEventFunc<T1, T2>(string funcKey, T1 para)
    {
        if (!GameEventGeneric<T1,T2>.dict2.ContainsKey(funcKey))
        {
            throw new KeyNotFoundException(funcKey + "not found");
        }
        return GameEventGeneric<T1,T2>.dict2[funcKey](para); //sus
    }
  
    public T1 OnEventFunc<T1>(string funcKey)
    {
        if (!GameEventGeneric<T1>.dict1.ContainsKey(funcKey))
        {
                    throw new KeyNotFoundException(funcKey + "not found");
        }
        return GameEventGeneric<T1>.dict1[funcKey]();
    }






    public void AddEvent(string stateName, Action action)
	{
        if (onStateNameToAction.ContainsKey(stateName))
        {
            onStateNameToAction[stateName] += action;
            return;
        }
        SavePrevEvent(stateName, action);
        onStateNameToAction.Add(stateName.ToString(), action);
    }
	public void AddEvent(Type stateName, Action action) //subscribe a action/method to a paticular object
	{
		if (onStateNameToAction.ContainsKey(stateName.ToString()))
		{
			onStateNameToAction[stateName.ToString()] += action;
			return;
		}
        SavePrevEvent(stateName.ToString(), action);
        onStateNameToAction.Add(stateName.ToString(), action);
    }


    public void AddEventFunc<T1, T2, T3>(string stateName, Func<T1, T2, T3> action) //subscribe a action/method to a paticular object
    {
        if (GameEventGeneric<T1, T2, T3>.dict3.ContainsKey(stateName))
        {
            throw new KeyNotFoundException("key" + stateName + "already contained");
        }
        GameEventGeneric<T1, T2, T3>.dict3.Add(stateName, action);
    }
    public void AddEventFunc<T1,T2>(string stateName, Func<T1,T2> action) //subscribe a action/method to a paticular object
    {
        if (GameEventGeneric<T1,T2>.dict2.ContainsKey(stateName))
        {
            throw new KeyNotFoundException("key" + stateName + "already contained");
        }
        GameEventGeneric<T1,T2>.dict2.Add(stateName, action);
    }
    public void AddEventFunc<T1>(string stateName, Func<T1> action) //subscribe a action/method to a paticular object
    {
        if (GameEventGeneric<T1>.dict1.ContainsKey(stateName))
        {
            throw new KeyNotFoundException("key" + stateName + "already contained");
        }
        GameEventGeneric<T1>.dict1.Add(stateName, action);
    }



    // -->  


}

