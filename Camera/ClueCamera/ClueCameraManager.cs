using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using ObserverAction;
using Persistence;
using UnityEditor.SearchService;
using UnityEngine;

public class ClueCameraManager : StaticInstance<ClueCameraManager>, IExecution, ISaveLoad, IObserverData<ObserverAction.PlayerActions, ClueMono>
{
    //scenename--cluecameraobject
    public Dictionary<SceneNames, List<ClueCameraObject>> sceneNameToClueIDToCameraObject = new();

    public Dictionary<int, RenderTexture> instanceIdToRenderTexture = new();
    public ClueCameraSpawner clueCameraSpawner { get; set; } //TODO

    public SystemActionCall<ClueCameraManager> onStartClueCameraManager = new();

    public int RTwidth { get; set; }
    public int RTheight { get; set; }
    private readonly int RTDEPTH = 24;

    public int clueCamerasMax { get; set; }

    public SubjectActionData<ObserverAction.ClueCameraActions, ClueMono> subject = new();
    //note
    public override void m_OnEnable()
    {
        PlayerData.INSTANCE?.playerRaycast.subject.AddObserver(this);
        MManager.INSTANCE?.onStartManagersAction.AddAction((MManager mm) => { mm.clueCameraManager = this; });
       
    }
    public override void m_Start()
    {
        
        onStartClueCameraManager.RunAction(this);
        base.m_Start();
    }
    public void AddCameraObject(SceneNames sceneNames, ClueCameraObject clueCameraObject)
    {
        if (!sceneNameToClueIDToCameraObject.ContainsKey(sceneNames))
        {
            sceneNameToClueIDToCameraObject.Add(sceneNames, new());
        }
        sceneNameToClueIDToCameraObject[sceneNames].Add(clueCameraObject);
    }
    public ClueCameraObject GetClueCameraObject(SceneNames sceneName, int clueIndex)
    {
        if (!sceneNameToClueIDToCameraObject.ContainsKey(sceneName))
        {
            Debug.LogError("cant find scene at" + sceneName);
            return null;
        }
        return sceneNameToClueIDToCameraObject[sceneName][clueIndex];
    }
    public int GetSceneIndexLength(SceneNames sceneName)
    {
        if (!sceneNameToClueIDToCameraObject.ContainsKey(sceneName))
        {
            Debug.LogError("cant find scene at" + sceneName);
            return -1;
        }
        return sceneNameToClueIDToCameraObject[sceneName].Count - 1;
    }
    public RenderTexture AddClueCameraFromOmit() //returns the render texture (what the camera is seeing) from spawned clue camera.
    {
        if (10 < clueCamerasMax)
        {

        }
        Vector3 position = PlayerData.INSTANCE.trans3d.position;
        Vector3 rotation = PlayerData.INSTANCE.trans3d.eulerAngles;
        //scene 1 -- cam 1, 2 ,3
        //scene 2 -- cam 4,5
        var currentScene = SceneData.INSTANCE.currentScene;

        GameObject g = clueCameraSpawner.SpawnClueCamera(position, rotation, currentScene, 0);


        //TODO might want to change this...

        var camRender = g.GetComponent<ClueCameraRender>();

        camRender.CreateClueCameraRender(RTwidth, RTheight, RTDEPTH);

        RenderTexture rt = camRender.renderTexture; //copy be reference, important!

        ClueCameraObject clueCameraObject = new(position, rotation, (int)currentScene, rt);

        AddCameraObject(SceneData.INSTANCE.currentScene, clueCameraObject);

        return rt;
    }
    //load clue camera before ledger images
    //IMPORTANT --> ledger image will get render texture  
    public void AddClueCameraFromLoad(/*json object params*/Vector3 position, Vector3 eulerRot, SceneNames sceneName, /*get it from path*/ Texture texture, int clueId)
    {
        //1-this code determines whether the json data scene number is our current scene number...
        //if yes, we will spawn the camera 
        //otherwise get the texture. 

        if (LedgerData.INSTANCE.ledgerImages.Count > 0)
        {
            Debug.LogWarning("are you sure clue camera is loading before ledger images? ");
        }
        if (sceneName == SceneData.INSTANCE.currentScene)
        {
            GameObject g = clueCameraSpawner.SpawnClueCamera(position, eulerRot, sceneName, clueId);
            Camera clueCamera = g.GetComponent<Camera>();

            RenderTexture rt = GetRenderTextureFromCamera(ref clueCamera);

            //add loaded clue camera object to dictionary...
            ClueCameraObject clueCameraObject = new(position, eulerRot, (int)sceneName, rt);
            AddCameraObject(SceneData.INSTANCE.currentScene, clueCameraObject);
        }
        else
        {
            ClueCameraObject clueCameraObject = new(position, eulerRot, (int)sceneName, texture);
            AddCameraObject(SceneData.INSTANCE.currentScene, clueCameraObject);
        }

    }
    private RenderTexture GetRenderTextureFromCamera(ref Camera cam)
    {
        //we have the texture
        RenderTexture ren = TextureHandler.INSTANCE?.CreateRenderTexture(RTwidth, RTheight, RTDEPTH);
        if (ren == null)
        {
            Debug.LogError("rt is null!");
            return null;
        }
        cam.targetTexture = ren; //we made our render texture!
        return ren;
    }


    public RenderTexture CreateRenderTexture()
    {
        return TextureHandler.INSTANCE?.CreateRenderTexture(RTwidth, RTheight, RTDEPTH);
    }


    //1- load BEFORE ledger images
    //2- add id to ledger image (determining what im)
    public void Load()
    {
        //load camera object data from json.
        List<JsonClueCamerasObject> jsonClueCamerasObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonClueCamerasObject>(FileNames.ClueCameraFile);
        jsonClueCamerasObjects.ForEach(x =>
        {


            foreach (var clueCamera in x.clueCameraObject)
            {
                Texture2D tex = TextureHandler.INSTANCE.GetTexture2D(clueCamera.textureName);
                AddClueCameraFromLoad(clueCamera.camPos, clueCamera.camEulerRot, (SceneNames)clueCamera.sceneId, tex, clueCamera.clueid);
            }

        });
        ///LoadDataFromFile<JsonLedgerImagesObject>(FileNames.LedgerImageFile)[0];



    }

    public (FileNames, JsonObject[])[] Save()
    {
        /*
        SceneNames[] sceneNames = sceneNameToClueIDToCameraObject.Keys.ToArray();
        List<JsonClueCamerasObject> jsonClueCameraObjects = new();
        foreach (var scene in sceneNames)
        {
            List<ClueCameraObject> clueCameraObjects = sceneNameToClueIDToCameraObject[scene];
            List<JsonClueCameraObject> jsonClueCameraObjects1 = new();

            //TODO -- convert all render texture to texutres... 
            TextureHandler.INSTANCE.SaveRenderTextureToPNG(clue)


            clueCameraObjects.ForEach(x => { jsonClueCameraObjects1.Add(new(x.camPos, x.camEulerRot,x.sceneId, x.)) });

            jsonClueCameraObjects.Add(new(scene, ));
        }

        return new (FileNames, JsonObject[])[]{
            (FileNames.ClueCameraFile, (JsonClueCameraObject[])jsonClueCameraObjects.ToArray())
        };
        */
        return null;

    }
    public void OnNotify(PlayerActions actionData, ClueMono myObject)
    {
        //on omit
        if (actionData == PlayerActions.onOmitRay)
        {
            AddClueCameraFromOmit(); //add to our dictionary!
            Debug.Log("camera spawned!");
            subject.NotifyObservers(ClueCameraActions.onSpawnCamera, myObject);
        }
    }
   
}