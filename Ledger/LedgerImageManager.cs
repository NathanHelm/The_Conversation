using System;
using System.Collections.Generic;
using Data;
using Persistence;
using PlasticGui.WorkspaceWindow;
using UI;
using UnityEngine;

public class LedgerImageManager : StaticInstance<LedgerImageManager>, ISaveLoad
{

    public LedgerImage temporaryImage { get; set; } = null;

    public static SystemActionCall<LedgerImageManager> onStartLedgerData = new SystemActionCall<LedgerImageManager>();
    [SerializeField]
    public List<LedgerImage> ledgerImages { get; set; } = new();
    public int MaxLedgerImageLength { get; set; }

    public override void OnEnable()
    {

        MManager.onStartManagersAction.AddAction(m => { m.ledgerImageManager = this; });
        base.OnEnable();
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

    //  public void 

    public void AddRayInfoToLedgerImage(string imageDescription, int questionID, int clueQuestionID, Texture ledgerImg, Texture[] ledgerOverlays, int clueBodyID) //converts ray information to ledger image object
    {
        LedgerImage ledgerImage = new(imageDescription, questionID, clueQuestionID, ledgerImg, ledgerOverlays, clueBodyID);

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
       
       
        List<JsonLedgerImageObject> ledgerImageObjects = new();

        foreach (LedgerImage single in ledgerImages)
        {
            ledgerImageObjects.Add(
                new(single.imageDescription,single.questionID, single.ledgerImage, single.clueQuestionID, single.ledgerOverlays)
            );
        }

            return new (FileNames, JsonObject[])[] {

            new(FileNames.LedgerImageFile, ledgerImageObjects.ToArray()

            ) };
    }

    public void Load()
    {

        List<JsonLedgerImageObject> jsonLedgerImageObjects = SavePersistenceManager.INSTANCE.LoadDataFromFile<JsonLedgerImageObject>(FileNames.LedgerImageFile);
        //here, we are getting the image objects from the persistent file and then trasferring them to the ledger images file... 

        foreach (JsonLedgerImageObject single in jsonLedgerImageObjects)
        {
            LedgerData.INSTANCE.ledgerImages.Add(new(single.imageDescription, single.questionID, single.clueQuestionID, single.ledgerImage, single.ledgerOverlays, single.clueBodyID));
        }
        LedgerManager.INSTANCE.ledgerImages = ledgerImages;
        LedgerManager.INSTANCE.AddImagesToLedgerPages();
        for(int i = 0; i < ledgerImages.Count; i++)
        {
            LedgerManager.INSTANCE.SetImageValueToOne(i);
        }
    }
}