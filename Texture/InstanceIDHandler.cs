using System.Collections.Generic;
using UnityEngine;

public class InstanceIDHandler<T> where T : Object{
   
    public Dictionary<int, T> dictionaryT = new Dictionary<int, T>();
     public void AddInstanceIDObject(T texture)
        {
            int instanceID = texture.GetInstanceID();
            if(dictionaryT.ContainsKey(instanceID))
            {
                dictionaryT[instanceID] = texture;
                return;
            }
            dictionaryT.Add(instanceID,texture);
        }

        public T GetInstanceIDObject(int instanceID)
        {
            if (dictionaryT.ContainsKey(instanceID))
            {
                return dictionaryT[instanceID];
            }
            else
            {
                Debug.LogError($"Texture with Instance ID {instanceID} not found.");
                return null;
            }
        }
}