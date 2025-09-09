using UnityEngine;
using System.IO;
namespace DictionaryHandler {

    public class ClueHandler : InstanceIDHandler<GameObject>, IExecution
    {
        public void m_Awake()
        {
            //throw new System.NotImplementedException();
        }

        public void m_OnEnable()
        {
            //throw new System.NotImplementedException();
        }

        public void m_GameExecute()
        {
            var clueMonos = FindObjectsByType<ClueMono>(FindObjectsSortMode.None);
            foreach (var single in clueMonos)
            {
                AddObjectToDict(single.clueID, single.gameObject);
            }
        }
        public GameObject GetClueByClueID(int key)
        {
            return GetKeyToObject(key);
        }
    }
}