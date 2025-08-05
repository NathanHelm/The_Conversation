using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

public class Tester_Memory : MonoBehaviour, IExecution
{
    [SerializeField]
    int[] memoryIds;
    [SerializeField]
    int[] subMemoryIds;
    private int characterId;

    [SerializeField]
    CharacterMono characterM;

    public void m_Awake()
    {
       
    }

    public void m_GameExecute()
    {


    }

    public void m_OnEnable()
    {
       characterId = 21
       ;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            foreach (var single in memoryIds)
            {
                MemoryManager.INSTANCE.UnlockMemory(characterId, single);
                foreach (var single1 in subMemoryIds)
                {
                    MemoryManager.INSTANCE.UnlockSubMemory(characterId, single , single1);
                }
                
            }

            SavePersistenceManager.INSTANCE.SaveFileInformation(MemoryManager.INSTANCE.Save());
            Debug.Log("UNLOCKED memory! for character--" + characterId);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("loaded stages from JSON--spawning stages");
            MemorySpawnerManager.INSTANCE.PopulateMemoryDictionary();
            MemorySpawnerManager.INSTANCE.Load();
        }
        
    }

}