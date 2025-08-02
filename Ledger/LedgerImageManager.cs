using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using ObserverAction;
using Persistence;
using PlasticGui.WorkspaceWindow;
using TMPro;
using UI;
using UnityEngine;
using System.Collections;
using UnityEditor.Graphs;
using System.Numerics;


public class LedgerImageManager : StaticInstance<LedgerImageManager>, ISaveLoad, IExecution, IObserverData<ObserverAction.ClueCameraActions, ClueMono>, IObserver<ObserverAction.LedgerActions>
{

    public LedgerImage temporaryImage { get; set; } = null;
    [SerializeField]
    public SobelMachine sobelMachine;
    [SerializeField]
    private Shader sobelFilter;
    [SerializeField]
    private float speed;
    public static SystemActionCall<LedgerImageManager> onStartLedgerData = new SystemActionCall<LedgerImageManager>();
    [SerializeField]
    public List<LedgerImage> ledgerImages { get; set; } = new();
    public int MaxLedgerImageLength { get; set; }
    private List<RenderTexture> postProcessingRenderTextures = new(); //renderTexture which will undergo graphics PP blit

    public override void m_OnEnable()
    {

        MManager.INSTANCE.onStartManagersAction.AddAction(m => { m.ledgerImageManager = this; });

        ClueCameraManager.INSTANCE?.subject.AddObserver(this);
        LedgerManager.INSTANCE?.subject.AddObserver(this);

        base.m_OnEnable();
    }
    public override void m_Start()
    {
        // base.m_Start();

        onStartLedgerData.RunAction(this);


    }
    public List<LedgerImage> GetLedgerImageList()
    {
        return ledgerImages;
    }


    public void AddLedgerImage(LedgerImage ledgerImage)
    {
        ledgerImages.Add(ledgerImage);

    }
    public void RemoveLedgerImage(int index)
    {
        ledgerImages.RemoveAt(index);
    }
    public void ReplaceImage(int index, LedgerImage newLedgerImage)
    {
        ledgerImages[index] = newLedgerImage;
    }
    public void SetTemporaryImageToNull()
    {
        temporaryImage = null;
    }
    public bool IsTemporyImageNull()
    {
        if (temporaryImage == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLedgerNull()
    {
        if (ledgerImages == null)
        {
            return true;
        }
        return false;
    }

    public int GetQuestionIDFromPage(int index)
    {
        return ledgerImages[index].questionID;
    }
    public int GetClueQuestionIDFromPage(int index)
    {
        return ledgerImages[index].clueQuestionID;
    }
    public int GetClueBodyIDFromPage(int index)
    {
        return ledgerImages[index].clueBodyID;
    }
    public bool IsIndexInLedgerImageListRange(int index)
    {
        if (ledgerImages == null)
        {
            return false;
        }
        if (ledgerImages.Count - 1 < index || index < 0)
        {
            return false;
        }
        return true;
    }
    //setting the a render's texture with the page image's texture on index
    public void SetRenderTextureToLedgerImage(ref Renderer renderer, int pageIndex)
    {
        if (!IsIndexInLedgerImageListRange(pageIndex))
        {
            Debug.LogError("pageIndex" + pageIndex + " not in range");
        }
        var currentLedgerimage = ledgerImages[pageIndex].ledgerImage;
        renderer.material.SetTexture("_MainTex", currentLedgerimage);
    }

    public void AddRayInfoToLedgerImage(int clueID, string imageDescription, int questionID, int clueQuestionID, Texture ledgerImg, Texture[] ledgerOverlays, int clueBodyID) //converts ray information to ledger image object
    {
        LedgerImage ledgerImage = new(clueID, imageDescription, questionID, clueQuestionID, ledgerImg, ledgerOverlays, clueBodyID);

        if (ledgerImages.Count >= MaxLedgerImageLength)
        {
            temporaryImage = ledgerImage;
            // GameEventManager.INSTANCE.OnEvent(typeof());
            return;
        }

        AddLedgerImage(ledgerImage);

        //TODO --> interesting.... I want to change the page's material BASED on the image drawn. (some function called image drawn sprite)
        //an idea: make drawing sprite THEN overlay it.
        //another idea = set drawing sprite as a texture and use some lerping function with the page. 

        // UI.LedgerUIManager.INSTANCE.ReplacePageSprite(LedgerData.INSTANCE.ledgerImages.Count - 1, ledgerImageSprite);

    }
    (FileNames, JsonObject[])[] ISaveLoad.Save()
    {


        JsonLedgerImagesObject ledgerImageObjects = new();
        ledgerImageObjects.ledgerImages = new JsonLedgerImageObject[ledgerImages.Count];

        for (int i = 0; i < ledgerImages.Count; i++)
        {

            ledgerImageObjects.ledgerImages[i] = new(
            ledgerImages[i].clueID,
            ledgerImages[i].imageDescription,
            ledgerImages[i].questionID,
            ledgerImages[i].clueQuestionID,
            ledgerImages[i].ledgerImage,
            ledgerImages[i].ledgerOverlays,
            ledgerImages[i].clueBodyID

            );
        }

        return new (FileNames, JsonObject[])[] {

            new(FileNames.LedgerImageFile, new JsonLedgerImagesObject[]{ ledgerImageObjects }

            ) };
    }

    public void Load()
    {

        JsonLedgerImagesObject jsonLedgerImageObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonLedgerImagesObject>(FileNames.LedgerImageFile)[0];
        //here, we are getting the image objects from the persistent file and getting its ledger images array.

        int n = jsonLedgerImageObjects.ledgerImages.Length;

        JsonLedgerImageObject[] jsonLedgerImageObjects1 = jsonLedgerImageObjects.ledgerImages;

        for (int i = 0; i < n; i++)
        {

            ClueCameraObject clueCameraObject = ClueCameraManager.INSTANCE.GetClueCameraObject(jsonLedgerImageObjects1[i].sceneName, jsonLedgerImageObjects1[i].clueCameraID);
            if (clueCameraObject == null)
            {
                Debug.LogError("clueCameraObject not found so we are removing image " + jsonLedgerImageObjects1[i].clueID);
                continue;
            }

            Texture tex = clueCameraObject.clueCameraTexture;

            AddLedgerImage(new(
            jsonLedgerImageObjects1[i].clueID,
            jsonLedgerImageObjects1[i].imageDescription,
            jsonLedgerImageObjects1[i].questionID,
            jsonLedgerImageObjects1[i].clueQuestionID,
            tex,
            jsonLedgerImageObjects1[i].ledgerOverlays,
            jsonLedgerImageObjects1[i].clueBodyID
            ));

            LedgerData.INSTANCE.ledgerImages = ledgerImages;
        }


        SetTexturesToLedgerUIPages();
        for (int i = 0; i < ledgerImages.Count; i++)
        {
            AddPostProcessingRenderTextures(ledgerImages[i]);
            SetImageValueToOne(i);
        }
    }
    public void SetImageValueToOne(int index)
    {
        Renderer ren = UI.LedgerUIManager.INSTANCE.GetImageObjectRenderer(index);
        if (ren == null)
        {
            Debug.LogError("image object renderer is null");
            return;
        }
        ren.material.SetFloat("_Val", 1);
    }

    public void OnNotify(ClueCameraActions actionData, ClueMono myObject)
    {
        if (actionData == ClueCameraActions.onSpawnCamera)
        {
            var currentScene = SceneData.INSTANCE.currentScene;
            //get clue ID from spawned camera
            int clueID = ClueCameraManager.INSTANCE.GetSceneIndexLength(currentScene);
            //get camera texture from spawned camera
            var CLUECAMERATEXTURE = ClueCameraManager.INSTANCE.GetClueCameraObject(currentScene, clueID).clueCameraTexture;

            AddRayInfoToLedgerImage(

            clueID,
            myObject.imageDescription,
            myObject.questionID,
            myObject.clueQuestionID,
            CLUECAMERATEXTURE,
            myObject.ledgerOverlays,
            ClueMono.clueBodyID
            
            );

            //image has been added, now run animations
        }
    }

    public void OnNotify(LedgerActions data)
    {
        if (data == LedgerActions.onSetTextureToPageImage)
        {
            SetTexturesToLedgerUIPages();
          //  AddPostProcessingRenderTextures(ledgerImages[^1]);
        }
    }

    public void SetTexturesToLedgerUIPages()
    {
        if (ledgerImages.Count == 0)
        {
            Debug.Log("LOG: ledger Images list has no ledgerimage's");
            return;
        }
        for (int i = 0; i < ledgerImages.Count; i++)
        {
            UI.LedgerUIManager.INSTANCE.SetTextureToPage(i, ledgerImages[i].ledgerImage);
        }


    }
    /*
    public IEnumerator test()
    {

        while (true)
        {
            // GL.Clear(true, true, Color.clear);
            for (int i = 0; i < ledgerImages.Count; i++)
            {
                //1) get the image
                if (ledgerImages[i].ledgerImage is not RenderTexture)
                {
                    //   continue;
                }
                var rt = ledgerImages[i].ledgerImage;
                RenderTexture rt2 = TextureHandler.INSTANCE.CreateRenderTexture(rt.height, rt.width, 24);
                Material mat = new Material(sobelFilter);
                if (i == 0)
                {
                    mat.SetFloat("_OutlineThickness", 10);
                    mat.SetFloat("_OutlineDepthMultiplier", 1);
                    mat.SetFloat("_OutlineDepthBias", 1);
                    mat.SetFloat("_OutlineNormalMultiplier", 1);
                    mat.SetFloat("_OutlineNormalBias", 1);
                }
                else
                {
                    mat.SetFloat("_OutlineThickness", 1000);
                }
                Graphics.Blit(rt, testing[i], mat);
                UI.LedgerUIManager.INSTANCE.SetTextureToPage(i, rt);
                RenderTexture.active = null;

            }

            //SetTexturesToLedgerUIPages();

            yield return new WaitForSeconds(1);
        }
    }*/
    
    public void AddPostProcessingRenderTextures(LedgerImage ledgerImage)
    {
        Debug.Log("my instance" + ledgerImage.ledgerImage.GetInstanceID());
        var rt = ledgerImage.ledgerImage;

        if (rt is not RenderTexture)
        {
            return;
        }

        var rt2 = TextureHandler.INSTANCE.CreateRenderTexture(rt.height, rt.width, 24);
        postProcessingRenderTextures.Add(rt2);
    }
}