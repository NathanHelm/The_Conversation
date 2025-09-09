using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codice.CM.Common;
using Data;
using DictionaryHandler;
using ObserverAction;
using Persistence;
using UnityEditor.SearchService;
using UnityEngine;

public class ClueCameraManager : StaticInstance<ClueCameraManager>, IExecution, ISaveLoad, IObserverData<ObserverAction.PlayerActions, ClueMono>, IObserver<ObserverAction.LedgerActions>
{
    private ClueHandler clueHandler;
    //scenename--cluecameraobject
    public Dictionary<SceneNames, Dictionary<int, ClueCameraObject>> sceneNameToLedgerImageKeyToClueIDToCameraObject = new();
    public Dictionary<int,ClueCameraObject> ledgerImageKeyToClueIDToCameraObject = new();

    public Dictionary<int, RenderTexture> instanceIdToRenderTexture = new();
    public ClueCameraSpawner clueCameraSpawner { get; set; } //TODO

    public SystemActionCall<ClueCameraManager> onStartClueCameraManager = new();

    public int RTwidth { get; set; }
    public int RTheight { get; set; }
    private readonly int RTDEPTH = 24;

    public int clueCamerasMax { get; set; }

    public SubjectActionData<ObserverAction.ClueCameraActions, (ClueCameraObject,ClueMono)> subject = new();
    //note
    public override void m_OnEnable()
    {
        PlayerData.INSTANCE?.playerRaycast.subjectClue.AddObserver(this);
        LedgerImageManager.INSTANCE.subject.AddObserver(this);
        MManager.INSTANCE?.onStartManagersAction.AddAction((MManager mm) => { mm.clueCameraManager = this; });

        clueHandler = gameObject.GetComponent<ClueHandler>();
        clueHandler ??= gameObject.AddComponent<ClueHandler>();
    }
    public override void m_Start()
    {
        
        onStartClueCameraManager.RunAction(this);
        base.m_Start();
    }
    public void AddCameraObject(SceneNames sceneNames, int ledgerImageKey, ClueCameraObject clueCameraObject)
    {
        if (!sceneNameToLedgerImageKeyToClueIDToCameraObject.ContainsKey(sceneNames))
        {
            sceneNameToLedgerImageKeyToClueIDToCameraObject.Add(sceneNames, new());
        }
        if (!ledgerImageKeyToClueIDToCameraObject.ContainsKey(ledgerImageKey))
        {
           ledgerImageKeyToClueIDToCameraObject.Add(ledgerImageKey, clueCameraObject);
        }
        sceneNameToLedgerImageKeyToClueIDToCameraObject[sceneNames].Add(ledgerImageKey, clueCameraObject);
 
    }
    public ClueCameraObject GetClueCameraObjectOnScene(SceneNames sceneName, int cluePK)
    {
        if (!sceneNameToLedgerImageKeyToClueIDToCameraObject.ContainsKey(sceneName))
        {
            Debug.LogError("cant find scene at" + sceneName);
            return null;
        }
        return sceneNameToLedgerImageKeyToClueIDToCameraObject[sceneName][cluePK];
    }
     public ClueCameraObject GetClueCameraObjectOnPK(int cluePK)
    {
        if (!ledgerImageKeyToClueIDToCameraObject.ContainsKey(cluePK))
        {
            Debug.LogError("cant find clue primary key at" + cluePK);
            return null;
        }
        return ledgerImageKeyToClueIDToCameraObject[cluePK];
    }
    public ClueCameraObject AddClueCameraFromOmit(ClueMono clueMono) //returns the render texture (what the camera is seeing) from spawned clue camera.
    {
        if (10 < clueCamerasMax)
        {

        }
        Vector3 position = PlayerData.INSTANCE.trans3d.position;
        Vector3 rotation = PlayerData.INSTANCE.trans3d.eulerAngles;
        //scene 1 -- cam 1, 2 ,3
        //scene 2 -- cam 4,5
        var currentScene = SceneData.CURRENTSCENE;

        GameObject g = clueCameraSpawner.SpawnClueCamera(position, rotation);


        //TODO might want to change this...

        var camRender = g.GetComponent<ClueCameraRender>();
        //add transform to camera look
         var camLook = g.GetComponent<ClueCameraLook>();

        if (clueHandler.GetClueByClueID(clueMono.clueID) == null)
        {
                Debug.LogError("no clue camera object to be found based on clue camera json file. id is ==>" + clueMono.clueID);
        }

        camLook.clueTrans = clueHandler.GetClueByClueID(clueMono.clueID)?.transform;
       // clueCameraLook.
        

        camRender.CreateClueCameraRender(RTwidth, RTheight, RTDEPTH);

        RenderTexture rt = camRender.renderTexture; //copy be reference, important!

        int hash = rt.GetHashCode();

        ClueCameraObject clueCameraObject = new(position, rotation, (int)currentScene, hash, clueMono.clueID , rt);

        AddCameraObject(SceneData.CURRENTSCENE, hash, clueCameraObject);


        return clueCameraObject;
    }
    //load clue camera AFFFTTTEERRR ledger images
    //IMPORTANT --> ledger image will get render texture  
    public void AddClueCameraFromLoad(/*json object params*/Vector3 position, Vector3 eulerRot, SceneNames sceneName, int PK,/*get it from path*/ Texture texture, int clueId)
    {
        //1-this code determines whether the json data scene number is our current scene number...
        //if yes, we will spawn the camera unless the primary key doesn't exist otherwise destroy it.

        //otherwise get the texture. 

        if (LedgerData.INSTANCE.ledgerImages.Count > 0)
        {
            Debug.LogWarning("are you sure clue camera is loading before ledger images? ");
        }
        if (sceneName == SceneData.CURRENTSCENE) //is scene texture or render texture? 
        {
            if (!LedgerImageManager.INSTANCE.DoesClueCameraExist(PK))
            {
                //remove clue camera from json because it does not exist in ledger image.
                return;
            }

            GameObject g = clueCameraSpawner.SpawnClueCamera(position, eulerRot);
            var camRender = g.GetComponent<ClueCameraRender>();
            camRender.CreateClueCameraRender(RTwidth, RTheight, RTDEPTH);

            //add transform to camera look
            var camLook = g.GetComponent<ClueCameraLook>();

            if (clueHandler.GetClueByClueID(clueId) == null)
            {
                Debug.LogError("no clue camera object to be found based on clue camera json file. id is ==>" + clueId);
            }

            camLook.clueTrans = clueHandler.GetClueByClueID(clueId)?.transform;
            
            RenderTexture rt = camRender.renderTexture;

            //add loaded clue camera object to dictionary...
            ClueCameraObject clueCameraObject = new(position, eulerRot, (int)sceneName, PK, clueId ,rt);
            AddCameraObject(SceneData.CURRENTSCENE,PK ,clueCameraObject);
        }
        else
        {
            ClueCameraObject clueCameraObject = new(position, eulerRot, (int)sceneName, PK, clueId ,texture);
            AddCameraObject(sceneName, PK ,clueCameraObject);
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


    //IMPORTANT: DO NOT USE CLUE CAMERA MANAGER LOAD, Observer does it for us.
    public void Load()
    {
        Debug.LogError("You are using clue camera manager load. don't do this.");
        //load camera object data from json.
        ///LoadDataFromFile<JsonLedgerImagesObject>(FileNames.LedgerImageFile)[0];



    }
    public void LoadObserver()
    {
         List<JsonClueCamerasObject> jsonClueCamerasObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonClueCamerasObject>(FileNames.ClueCameraFile);
        jsonClueCamerasObjects.ForEach(x =>
        {
            foreach (var clueCamera in x.clueCameraObject)
            {
                Texture2D tex = TextureHandler.INSTANCE.GetTextureAbsolute(clueCamera.textureName);
                AddClueCameraFromLoad(clueCamera.camPos, clueCamera.camEulerRot, (SceneNames)clueCamera.sceneId, clueCamera.clueCameraPrimaryKey, tex, clueCamera.clueId);
            }

        });
    }

    public (FileNames, JsonObject[])[] Save()
    {
        

        SceneNames[] sceneNames = sceneNameToLedgerImageKeyToClueIDToCameraObject.Keys.ToArray();
        List<JsonClueCameraObject> jsonClueCameraObjects = new();
        List<JsonClueCamerasObject> jsonClueCamerasObjects = new();
        foreach (var single in sceneNames)
        {
            int[] clueCameraPrimaryKey = sceneNameToLedgerImageKeyToClueIDToCameraObject[single].Keys.ToArray();
            foreach (var single1 in clueCameraPrimaryKey)
            {
                string path = "Image_" + single1.ToString();

                ClueCameraObject clueCameraObject = sceneNameToLedgerImageKeyToClueIDToCameraObject[single][single1] as ClueCameraObject;

                if (sceneNameToLedgerImageKeyToClueIDToCameraObject[single][single1].clueCameraTexture is RenderTexture)
                {
                    //save texture to assets
                    TextureHandler.INSTANCE.SaveRenderTextureToPNG((RenderTexture)sceneNameToLedgerImageKeyToClueIDToCameraObject[single][single1].clueCameraTexture, path);
                }

                jsonClueCameraObjects.Add(new JsonClueCameraObject(clueCameraObject.camPos, clueCameraObject.camEulerRot, clueCameraObject.sceneId, path, clueCameraObject.clueCameraPrimaryKey, clueCameraObject.clueId));

            }
        }
        jsonClueCamerasObjects.Add(new JsonClueCamerasObject(jsonClueCameraObjects.ToArray()));
        return new (FileNames, JsonObject[])[]{
        (FileNames.ClueCameraFile, jsonClueCamerasObjects.ToArray()),
        };

      
        /*SceneNames[] sceneNames = sceneNameToClueIDToCameraObject.Keys.ToArray();
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

    }
    public void OnNotify(PlayerActions actionData, ClueMono myObject)
    {
        //on omit
        if (actionData == PlayerActions.onOmitRayClue)
        {
            var clueCameraObj = AddClueCameraFromOmit(myObject); //add to our dictionary!
            Debug.Log("camera spawned!");
            subject.NotifyObservers(ClueCameraActions.onSpawnCamera,new(clueCameraObj,myObject));
        }
    }

    public void OnNotify(LedgerActions data)
    {
        if (data == LedgerActions.onAddedPrimaryKeyToLedgerImage)
        {
            LoadObserver();
        }
    }
}