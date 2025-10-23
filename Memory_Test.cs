using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory_Test : MonoBehaviour
{
    [SerializeField]
    MemorySpawnerManager memorySpawner;
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("unlock memory spawner!");
            MemoryManager.INSTANCE.UnlockMemory(24, 1);
            MemoryManager.INSTANCE.Save();

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            memorySpawner.Load();
        }
    }
}
