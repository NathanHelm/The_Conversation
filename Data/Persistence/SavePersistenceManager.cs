
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Persistence;
using UnityEngine;


public enum FileNames { myfile, mydialogfile, DialogueConversationFile, InterviewFile, LedgerImageFile, PlayerFile, SpawnFile, MemoryFile, UnlockedMemoryFile, ClueCameraFile }; //enter all filenames here

/*
so you want to save somethings
1- add enum
2- add the correct casting in interface implemented function  
3- see PopulateCurrentFiles()
4- see MakePersistenceDictionary()
5- see SaveFileInformation()

*/
public class SavePersistenceManager : StaticInstance<SavePersistenceManager>, IExecution
{
    //handles ../../../JSON
    public readonly string jsonPath = "JSONFILES";

    private Dictionary<FileNames, Dictionary<int, JsonObject>> fileNameToJsonIDToObject = new Dictionary<FileNames, Dictionary<int, JsonObject>>();

    private Dictionary<FileNames, object> fileHandlerNameTofileHandler = new Dictionary<FileNames, object>();
    public ISaveLoad[] SaveLoads { get; set; } //obtain all Isaveload


    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) => { m.savePersistenceManager = this; });
        base.m_OnEnable();
    }
    public override void m_Start()
    {
        if (fileHandlerNameTofileHandler.Count == 0)
        {
           PopulateCurrentFiles();
        }
         MakePersistenceDictionary(); //making persistent dictionary from json filehandlers

    }

    public string GetPath()
    {
        string assetsDataPath = Application.dataPath;
        string path = Path.Combine(assetsDataPath, jsonPath);
        return path;
    }
    public void PopulateCurrentFiles()
    {
        
        /*
        Add all Filehandlers to hashmap fileHandlerNameTofileHandler...
        */
        //=======================================================================================================================
        FileHandler<JsonObject> fileHandler = new FileHandler<JsonObject>(Path.Combine(GetPath(), FileNames.myfile + ".json"));

        fileHandlerNameTofileHandler.Add(FileNames.myfile, fileHandler);
        //========================================================================================================================
        FileHandler<JsonQuestionObject> fileHandler1 = new FileHandler<JsonQuestionObject>(Path.Combine(GetPath(), FileNames.mydialogfile + ".json"));

        fileHandlerNameTofileHandler.Add(FileNames.mydialogfile, fileHandler1);
        //========================================================================================================================
        //DialogueConversationFile
        FileHandler<JsonDialogueConversationObject> fileHandler2 = new FileHandler<JsonDialogueConversationObject>(Path.Combine(GetPath(), FileNames.DialogueConversationFile + ".json"));

        fileHandlerNameTofileHandler.Add(FileNames.DialogueConversationFile, fileHandler2);
        //========================================================================================================================

        FileHandler<JsonInterviewObject> fileHandler3 = new FileHandler<JsonInterviewObject>(Path.Combine(GetPath(), FileNames.InterviewFile + ".json"));
        fileHandlerNameTofileHandler.Add(FileNames.InterviewFile, fileHandler3);

        FileHandler<JsonLedgerImagesObject> fileHandler4 = new FileHandler<JsonLedgerImagesObject>(Path.Combine(GetPath(), FileNames.LedgerImageFile + ".json"));
        fileHandlerNameTofileHandler.Add(FileNames.LedgerImageFile, fileHandler4);

        FileHandler<JsonPlayerObject> fileHandler5 = new FileHandler<JsonPlayerObject>(Path.Combine(GetPath(), FileNames.PlayerFile + ".json"));
        fileHandlerNameTofileHandler.Add(FileNames.PlayerFile, fileHandler5);

         FileHandler<JsonSpawnObject> fileHandler6 = new FileHandler<JsonSpawnObject>(Path.Combine(GetPath(), FileNames.SpawnFile + ".json"));
        fileHandlerNameTofileHandler.Add(FileNames.SpawnFile, fileHandler6);

        FileHandler<JsonUnlockMemoryObject> fileHandler7 = new FileHandler<JsonUnlockMemoryObject>(Path.Combine(GetPath(), FileNames.UnlockedMemoryFile + ".json"));
        fileHandlerNameTofileHandler.Add(FileNames.UnlockedMemoryFile, fileHandler7);

        FileHandler<JsonMemoryObject> fileHandler8 = new FileHandler<JsonMemoryObject>(Path.Combine(GetPath(), FileNames.MemoryFile + ".json"));
        fileHandlerNameTofileHandler.Add(FileNames.MemoryFile, fileHandler8);

        FileHandler<JsonClueCamerasObject> fileHandler9 =  new FileHandler<JsonClueCamerasObject>(Path.Combine(GetPath(), FileNames.ClueCameraFile + ".json"));
        fileHandlerNameTofileHandler.Add(FileNames.ClueCameraFile, fileHandler9);
         //dont touch this.
        foreach (FileNames fileHandlerKey in fileHandlerNameTofileHandler.Keys)
        {

            fileNameToJsonIDToObject.Add(fileHandlerKey, new Dictionary<int, JsonObject>());

        }

        //Todo continue this for other existing files...

    }
    public void PopulatePersistenceDictionary<T>(FileNames key) where T : JsonObject
    {
        FileHandler<T> fileHandler = (FileHandler<T>)fileHandlerNameTofileHandler[key];
        fileHandler.GetObjectsFromFile();
        List<T> jsonObjects = fileHandler.GetGameObjectFromStack();

        for (int i = 0; i < jsonObjects.Count; i++)
        {
            if (!fileNameToJsonIDToObject.ContainsKey(key))
            {
                UnityEngine.Debug.LogError("filename key " + key + " is not there");
                continue;
            }
            if (jsonObjects[i] is JsonObject)
            {
                T jsonObject = (T)jsonObjects[i];
                if (fileNameToJsonIDToObject[key].ContainsKey(jsonObject.Id))
                {
                    Debug.LogError("JSON ID " + jsonObject.Id + " already exists!");
                    continue;
                }
                fileNameToJsonIDToObject[key].Add(jsonObject.Id, jsonObject);
            }
            else
            {
                UnityEngine.Debug.LogError("instance is not of type jsonobject");
            }
        }
    }
    public void MakePersistenceDictionary()
    {
        /*
        Add all object store in the json file to our hashmap to ensure that duplcate json object are not added. 
        this code MUST run before anything else to ensure data consistency.  
        TODO: add this stuff to MManager.
        */
        foreach (FileNames fileHandlerKey in fileHandlerNameTofileHandler.Keys)
        {

            object jsonObject = fileHandlerNameTofileHandler[fileHandlerKey];
            if (jsonObject is FileHandler<JsonDialogueConversationObject>)
            {
                PopulatePersistenceDictionary<JsonDialogueConversationObject>(fileHandlerKey);
            }
            //=memory===================================================================================================================
            else if (jsonObject is FileHandler<JsonUnlockMemoryObject>)
            {
                PopulatePersistenceDictionary<JsonUnlockMemoryObject>(fileHandlerKey);
            }
            else if (jsonObject is FileHandler<JsonMemoryObject>)
            {
                PopulatePersistenceDictionary<JsonMemoryObject>(fileHandlerKey);
            }
            //===========================================================================================================================
            else if (jsonObject is FileHandler<JsonQuestionObject>)
            {
                PopulatePersistenceDictionary<JsonQuestionObject>(fileHandlerKey);
            }
            else if (jsonObject is FileHandler<JsonInterviewObject>)
            {
                PopulatePersistenceDictionary<JsonInterviewObject>(fileHandlerKey);
            }
            else if (jsonObject is FileHandler<JsonLedgerImagesObject>)
            {
                PopulatePersistenceDictionary<JsonLedgerImagesObject>(fileHandlerKey);
            }
            else if (jsonObject is FileHandler<JsonPlayerObject>)
            {
                PopulatePersistenceDictionary<JsonPlayerObject>(fileHandlerKey);
            }
            else if (jsonObject is FileHandler<JsonSpawnObject>)
            {
                PopulatePersistenceDictionary<JsonSpawnObject>(fileHandlerKey);
            }
            else if (jsonObject is FileHandler<JsonClueCamerasObject>)
            {
                PopulatePersistenceDictionary<JsonClueCamerasObject>(fileHandlerKey);
            }
            else if (jsonObject is FileHandler<JsonObject>) //run last
            {
                PopulatePersistenceDictionary<JsonObject>(fileHandlerKey);
            }
            else
            {
                UnityEngine.Debug.LogError("Not type json object");
            }
            //iterate through file handlers and adds it to the dictionary

        }
    }

    public bool IsJsonDuplicate(int id, FileNames fileName)
    {
        if (fileNameToJsonIDToObject.ContainsKey(fileName))
        {
            if (fileNameToJsonIDToObject[fileName].ContainsKey(id))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void SaveToFileName<T>(FileNames key, T[] saveObj) where T : JsonObject
    {
        FileHandler<T> fileHandler = (FileHandler<T>)fileHandlerNameTofileHandler[key];

        foreach (T jsonObject in saveObj)
        {

            if (IsJsonDuplicate(jsonObject.Id, key))  /*if true, it means that json with same id is trying to be added*/
            {
                UnityEngine.Debug.Log("Json id already used with id " + jsonObject.Id);
                continue;
            }

            fileNameToJsonIDToObject[key].Add(jsonObject.Id, jsonObject);

            SaveDataToFileHandler(jsonObject, ref fileHandler);
            UnityEngine.Debug.Log("Json added to file");

        }

        fileHandler.PutToFile();
    }
    public void ReplaceFileName<T>(FileNames key, T[] replaceObj) where T : JsonObject
    {
        //note that you can't have an array of T because we are not adding multiple components of the same data only on json data.
        //also, the ID doesn't matter as we are only storing a single json object to the file...
        FileHandler<T> fileHandler = (FileHandler<T>)fileHandlerNameTofileHandler[key];

        fileNameToJsonIDToObject[key].Clear();

        foreach (T single in replaceObj)
        {
            SaveDataToFileHandler(single, ref fileHandler);
            fileNameToJsonIDToObject[key].Add(single.Id, single);
        }

        fileHandler.ReplaceFile();

    }

    public void Save()
    {
        //obtain all interfaces
        // TODO continue with this saving function... 

        SaveLoads ??= FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveLoad>().ToArray();

        foreach (ISaveLoad single in SaveLoads) //iterate through every object that is being saved
        {
            (FileNames, JsonObject[])[] saveObjects = single.Save();

            SaveFileInformation(saveObjects);

        }

    }

    public void Load()
    {
        SaveLoads ??= FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveLoad>().ToArray();

        foreach (ISaveLoad saveLoad in SaveLoads)
        {
            saveLoad.Load();
        }
    }
    public void SaveDataToFileHandler<T>(T jsonObject, ref FileHandler<T> fileHandler) where T : JsonObject
    {
        fileHandler.AddData(jsonObject);
    }
    public List<T> LoadDataFromFile<T>(FileNames fileName) where T : JsonObject
    {
        FileHandler<T> fileHandler = fileHandlerNameTofileHandler[fileName] as FileHandler<T>;

        fileHandler.GetObjectsFromFile();
        List<T> temp = fileHandler.GetGameObjectFromStack();
        return temp;
    }

    //==============================================================================================================

    public void SaveInterfaceData(ISaveLoad saveLoad)
    {
        if (saveLoad == null)
        {
            Debug.LogError("could not find instance therefore we cannot run save interface");
            return;
        }
        (FileNames, JsonObject[])[] saveObject = saveLoad.Save();

        if (saveObject == null)
        {
            Debug.LogError("save object is null");
            return;
        }

        SaveFileInformation(saveObject);

    }
    
    //==============================================================================================================

    public void SaveFileInformation((FileNames, JsonObject[])[] saveObjects)
    {
        foreach ((FileNames, JsonObject[]) saveObj in saveObjects)
        {
            FileNames currentFile = saveObj.Item1;

            //NOTE: json that adds data to an existing file is 'SaveTofileName.' However, json that is replaces data from an existing file is of function type 'ReplaceFileName.'
            if (saveObj.Item2 is JsonDialogueConversationObject[])
            {
                SaveToFileName(currentFile, (JsonDialogueConversationObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonUnlockMemoryObject[])
            {
                ReplaceFileName(currentFile, (JsonUnlockMemoryObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonMemoryObject[])
            {
                ReplaceFileName(currentFile, (JsonMemoryObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonQuestionObject[])
            {
                SaveToFileName(currentFile, (JsonQuestionObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonInterviewObject[])
            {
                //note you can only have one json interview object...
                //update: we will make an array on size one...
                ReplaceFileName(currentFile, (JsonInterviewObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonClueCamerasObject[])
            {
                ReplaceFileName(currentFile, (JsonClueCamerasObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonLedgerImagesObject[])
            {
                ReplaceFileName(currentFile, (JsonLedgerImagesObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonPlayerObject[])
            {
                ReplaceFileName(currentFile, (JsonPlayerObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonSpawnObject[])
            {
                ReplaceFileName(currentFile, (JsonSpawnObject[])saveObj.Item2);
            }
            else if (saveObj.Item2 is JsonObject[])
            {
                SaveToFileName(currentFile, saveObj.Item2);
            }
            else
            {
                UnityEngine.Debug.LogError("Not type json object");
            }

        }
    }



}
