using System;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;
public class FileHandler<T> where T : JsonObject
{
    /*
    - Each file holder represents a JSON file
    - functions send data to a file
    - It also accordingly transfers data BACK to a Gameobject. 
    */

   
    Stack<T> gameObjectsInJson = new Stack<T>(); //gameobject that will put & retrieve data 
    List<T> addToJsonList = new List<T>();

    public string Path {get; set;} //file location that contains data. 

    public FileHandler(string path)
    {
        this.Path = path;
    }
   


    
  
    private bool IsGameObjectValid(object game)
    {
        if(game == null)
        {
            Debug.LogError("Gameobject is null");
            return false;
        }
        return true;
    }
    private bool IsJSONValid(string json)
    {
        
        if(string.IsNullOrEmpty(json))
        {
            Debug.LogError("There is no JSON.");
            return false;
        }
        return true;
    }

    public void GetObjectsFromFile() //1) get objects from file and add to stack
    {
        //gets json object from file and adds it to 
        string jsonFromFile = File.ReadAllText(Path);

        if(string.IsNullOrEmpty(jsonFromFile))
        {
            Debug.LogError("no json found in file");
           return;
        }

        T[] JsonArray = JsonUtility.FromJson<Wrapper<T>>(jsonFromFile).Items;

        if(JsonArray == null)
        {
            return;
        }
       
        foreach(T single in JsonArray)
        {
            gameObjectsInJson.Push(single);
        }
        
    }
    public List<T> GetGameObjectFromStack() //2) 
    {
        List<T> temp = new List<T>();
        if(gameObjectsInJson.Count == 0)
        {
            Debug.LogError("there are no json objects");
        }
        while(gameObjectsInJson.Count > 0)
        {
            temp.Add(gameObjectsInJson.Pop());
        }
        return temp;
    }
    public void AddData(T data)
    {
        addToJsonList.Add(data);
    }
    public void PutToFile()
    {
        //1) make gameobject to json

        //2) if path does not exist: make a file. 
        GetObjectsFromFile();
        List<T> currentObjectsInfile = GetGameObjectFromStack();

        for(int i = 0; i < currentObjectsInfile.Count; i++)
        {
            addToJsonList.Add(currentObjectsInfile[i]);
        }
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = addToJsonList.ToArray();
        string json = JsonUtility.ToJson(wrapper, true);
        CreateFile(Path, json); //add to json file...
        addToJsonList.Clear();
    }
    private void CreateFile(string path, string json){
         File.WriteAllText(path, json); //adding json to a CREATED file. 
    }

   

}

[Serializable]
public class Wrapper<T>
{
    public T[] Items;
}
