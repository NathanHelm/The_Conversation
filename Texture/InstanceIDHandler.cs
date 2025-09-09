using System.Collections.Generic;
using UnityEngine;

public class InstanceIDHandler<T>: MonoBehaviour where T : Object{
   
    public Dictionary<int, T> dictionaryT = new Dictionary<int, T>();
     public void AddObjectToDict(int key,T value)
        {
            if(dictionaryT.ContainsKey(key))
            {
                dictionaryT[key] = value;
                return;
            }
            dictionaryT.Add(key,value);
        }

        public T GetKeyToObject(int key)
        {
            if (dictionaryT.ContainsKey(key))
            {
                return dictionaryT[key];
            }
            else
            {
                Debug.LogError($"key {key} not found.");
                return null;
            }
        }
}