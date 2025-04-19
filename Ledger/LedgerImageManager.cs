using System;
using System.Collections.Generic;
using Data;
using PlasticGui.WorkspaceWindow;
using UnityEngine;

public class LedgerImageManager : StaticInstance<LedgerImageManager>{
    
    public LedgerImage temporaryImage {get; set;} = null;

    public static SystemActionCall<LedgerImageManager> onStartLedgerData = new SystemActionCall<LedgerImageManager>();

    public List<LedgerImage> ledgerImages {get; set;} = new();
    public int MaxLedgerImageLength {get; set;}

    public override void OnEnable()
    {
        //TODO add to mmanager
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
    public bool IsTemporyImageNull()
    {
       if(temporaryImage == null)
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
        if(ledgerImages == null)
        {
            return true;
        }
        return false;
    }
    public int[] GetQuestionsIDFromPage(int index)
    {
       return ledgerImages[index].customQuestions;
    }
  //  public void 

     public void AddRayInfoToLedgerImage(int bodyId, string dialogueDescription, int[] customQuestions, Texture ledgerImg, int[] memoryID) //converts ray information to ledger image object
    {
        LedgerImage ledgerImage = new(dialogueDescription, customQuestions, bodyId, ledgerImg, memoryID);
        
        if(ledgerImages.Count > MaxLedgerImageLength)
        {
            temporaryImage = ledgerImage;
            return;
        }

        AddLedgerImage(ledgerImage);

        //TODO --> interesting.... I want to change the page's material BASED on the image drawn. (some function called image drawn sprite)
        //an idea: make drawing sprite THEN overlay it.
        //another idea = set drawing sprite as a texture and use some lerping function with the page. 

       // UI.LedgerUIManager.INSTANCE.ReplacePageSprite(LedgerData.INSTANCE.ledgerImages.Count - 1, ledgerImageSprite);
        
    }


}