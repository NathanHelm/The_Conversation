using System.Collections;
using System.Collections.Generic;
using Persistence;
using UnityEngine;

public class TesterSavingLoadingDummy : MonoBehaviour
{
    [SerializeField]
    SavePersistenceManager savePersistenceManager;
    [SerializeField]
    private List<JsonObject> retrievedJsonObjItems = new List<JsonObject>();
    [SerializeField]
    private List<JsonQuestionObject> retrievedJsonQuestionObjItems = new List<JsonQuestionObject>();
    // Update is called once per frame
    void Start()
    {
        savePersistenceManager = SavePersistenceManager.INSTANCE;
        /*
        savePersistenceManager.PopulateCurrentFiles();
        savePersistenceManager.MakePersistenceDictionary(); //making persistent dictionary from json filehandlers
        savePersistenceManager.Load();
        */
    }
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.I)){
            Debug.Log("save");
            savePersistenceManager.Save();

        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("load");
            List<JsonObject> jsonObjects = savePersistenceManager.LoadDataFromFile<JsonObject>(FileNames.myfile);
            retrievedJsonObjItems = jsonObjects;

            List<JsonQuestionObject> dialoguejsonObjects = savePersistenceManager.LoadDataFromFile<JsonQuestionObject>(FileNames.mydialogfile);
            retrievedJsonQuestionObjItems = dialoguejsonObjects;
            
        }
        */

         if(Input.GetKeyDown(KeyCode.I)){
            Debug.Log("save");
            savePersistenceManager.Save();

        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("load");
            savePersistenceManager.Load();
        }
    }
}
