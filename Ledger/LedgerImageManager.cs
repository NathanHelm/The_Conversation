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
using UnityEditor;


public class LedgerImageManager : StaticInstance<LedgerImageManager>, ISaveLoad, IExecution, IObserverData<ObserverAction.ClueCameraActions,(ClueCameraObject,ClueMono)>, IObserver<ObserverAction.LedgerActions>
{

    public LedgerImage temporaryImage { get; set; } = null;
    [SerializeField]
    public SobelMachine sobelMachine;
    [SerializeField]
    private Shader sobelFilter;
    [SerializeField]
    private float speed;
    public static SystemActionCall<LedgerImageManager> onStartLedgerData = new SystemActionCall<LedgerImageManager>();

    public Dictionary<int, LedgerImage> keyToLedgerImage = new Dictionary<int, LedgerImage>();
    [SerializeField]
    public List<LedgerImage> ledgerImages { get; set; } = new();
    public int MaxLedgerImageLength { get; set; }
    private List<RenderTexture> postProcessingRenderTextures = new(); //renderTexture which will undergo graphics PP blit

    public Subject<ObserverAction.LedgerActions> subject = new();
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
    public void PopulatePkKeyToLedgerImage(int ClueCameraSpawnPK, LedgerImage pairImage)
    {
        if (keyToLedgerImage.ContainsKey(ClueCameraSpawnPK))
        {
            Debug.LogError("duplicate Hash primary keys. This should honest be impossible.");
            return;
        }
        keyToLedgerImage.Add(ClueCameraSpawnPK, pairImage);
    }
    public bool DoesClueCameraExist(int ClueCameraSpawnPK)
    {
        return keyToLedgerImage.ContainsKey(ClueCameraSpawnPK);
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

    public void AddRayInfoToLedgerImage(int clueID, string imageDescription, int questionID, Texture ledgerImg, int clueBodyID, SceneNames sceneName, int clueCameraForeignKey) //converts ray information to ledger image object
    {
        LedgerImage ledgerImage = new(clueID, imageDescription, questionID, ledgerImg, clueBodyID, sceneName, clueCameraForeignKey);

        if (ledgerImages.Count >= MaxLedgerImageLength)
        {
            temporaryImage = ledgerImage;
            // GameEventManager.INSTANCE.OnEvent(typeof());
            return;
        }

        AddLedgerImage(ledgerImage);
        PopulatePkKeyToLedgerImage(clueCameraForeignKey, ledgerImage);

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
       
            ledgerImages[i].imageDescription,
            ledgerImages[i].questionID,
            ledgerImages[i].ledgerImage,
            null,
            -1,
            ledgerImages[i].clueID,
            ledgerImages[i].clueBodyID,
            ledgerImages[i].sceneName,
            ledgerImages[i].clueCameraForiegnKey


            );
        }

        return new (FileNames, JsonObject[])[] {

            new(FileNames.LedgerImageFile, new JsonLedgerImagesObject[]{ ledgerImageObjects }

            ) };
    }

    public void Load()
    {
        if (SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonLedgerImagesObject>(FileNames.LedgerImageFile).Count == 0)
        {
            Debug.LogWarning("there is no ledger image data");
            return;
        }
        JsonLedgerImagesObject jsonLedgerImageObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonLedgerImagesObject>(FileNames.LedgerImageFile)[0];
        //here, we are getting the image objects from the persistent file and getting its ledger images array.

        int n = jsonLedgerImageObjects.ledgerImages.Length;

        JsonLedgerImageObject[] jsonLedgerImageObjects1 = jsonLedgerImageObjects.ledgerImages;

        Texture dummy = null; //TODO add some image so we can easily identify that the clue camera object has not been loaded correctly...

        for (int i = 0; i < n; i++)
        {
            LedgerImage ledgerImage = new(

            jsonLedgerImageObjects1[i].clueID,
            jsonLedgerImageObjects1[i].imageDescription,
            jsonLedgerImageObjects1[i].questionID,

            dummy,
            jsonLedgerImageObjects1[i].clueBodyID,
            jsonLedgerImageObjects1[i].sceneName,
            jsonLedgerImageObjects1[i].clueCameraForeignKey

            );
            AddLedgerImage(ledgerImage);
            PopulatePkKeyToLedgerImage(jsonLedgerImageObjects1[i].clueCameraForeignKey, ledgerImage);
            
        }
        //notify observer (load clue camera manager)... 
        subject.NotifyObservers(LedgerActions.onAddedPrimaryKeyToLedgerImage);

        for (int i = 0; i < n; i++)
        {

            ClueCameraObject clueCameraObject = ClueCameraManager.INSTANCE.GetClueCameraObjectOnPK(jsonLedgerImageObjects1[i].clueCameraForeignKey);

            if (clueCameraObject == null)
            {
                Debug.LogError("clueCameraObject not found so we are removing image " + jsonLedgerImageObjects1[i].clueID);
                ledgerImages.RemoveAt(i);
                continue;
            }

            Texture tex = clueCameraObject.clueCameraTexture;
            ledgerImages[i].ledgerImage = tex;

        }

        LedgerData.INSTANCE.ledgerImages = ledgerImages;
        SetTexturesToLedgerUIPages();
        for (int i = 0; i < ledgerImages.Count; i++)
        {
          //  AddPostProcessingRenderTextures(ledgerImages[i]);
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

    public void OnNotify(ClueCameraActions actionData, (ClueCameraObject,ClueMono) myObject)
    {
        if (actionData == ClueCameraActions.onSpawnCamera)
        {
            var currentScene = SceneData.CURRENTSCENE;
            //get clue ID from spawned camera
            int primaryKey = myObject.Item1.clueCameraPrimaryKey;
            //get camera texture from spawned camera
            var CLUECAMERATEXTURE = ClueCameraManager.INSTANCE.GetClueCameraObjectOnScene(currentScene, primaryKey).clueCameraTexture;
            AddRayInfoToLedgerImage(

            primaryKey,
            myObject.Item2.imageDescription,
            myObject.Item2.questionID,
            CLUECAMERATEXTURE,
            ClueMono.clueBodyID,
            currentScene,
            primaryKey
            
            
            );

            //image has been added, now run animations
        }
    }

    public void OnNotify(LedgerActions data)
    {
        if (data == LedgerActions.onSetTextureToPageImage)
        {
            SetTexturesToLedgerUIPages();
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