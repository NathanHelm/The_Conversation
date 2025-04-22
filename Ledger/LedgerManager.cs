using UnityEngine;
using System.Collections;
using System;
using Data;
using System.Collections.Generic;
using UI;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using Codice.Client.BaseCommands.TubeClient;
using Codice.Client.BaseCommands;

public class LedgerManager : StaticInstance<LedgerManager>
{
    public static SystemActionCall<LedgerManager> onStartLedgerData = new SystemActionCall<LedgerManager>();
    public static SystemActionCall<LedgerManager> onActiveLedger = new SystemActionCall<LedgerManager>();

    public List<LedgerImage> ledgerImages = new List<LedgerImage>();
    
    public int index{get; set;} = 0;
    int rotateIndex = 0;

    public readonly int ledgerLength = 10;

    public bool isLedgerCreated = true;
    
    int pageL; 

    int furthestRight; //index of furthest-most ledger image

    public bool edgechecker {get; private set;} = true;

    public bool isLeft {get; set;} = false;

    [SerializeField]
    float speed = 2.7f; 
    float animationSpeed;

    bool runOnce = true;


  

    public override void OnEnable()
    {
       // Debug.LogError("OI bruv its time you lock tf in and get pages rights for ledger manager.");
     //   MManager.onStartManagersAction?.AddAction((MManager m) => { m.ledgerManager = this; });
        base.OnEnable();
    }


    public override void m_Start()
    {
        LedgerUIManager.INSTANCE.onFlipAt90Degrees.AddPriorityAction(((int,LedgerUIManager) ledgerManagerUI) => { ChangeColorAndLayering(index) ;});
        furthestRight = 2 * (ledgerLength - 2);
        base.m_Start();
        onStartLedgerData.RunAction(this);
         
    }
    public void UseLedgerState()
    {
        //Pressing tab opens the ledger
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            OpenLedgerState();
        }

    }
    private void RunLedger()
    {
       
        if(isLedgerCreated)
        {
            CreateLedger(); //make a ten page ledger
            isLedgerCreated = false;
        }

        pageL = UI.LedgerUIManager.INSTANCE.GetPageLength();
        LedgerImageManager.INSTANCE.MaxLedgerImageLength = pageL;

        animationSpeed = (speed / pageL) ;

        
        onActiveLedger.RunAction(this); //for one, upadte ledgerimages list...
        //AddImagesToLedgerPages();


        UI.LedgerUIManager.INSTANCE.FlipPageRight(0);
        ChangeColorLayeringBorderLeft();

      
 
    }
    //==run on state start ==================================================================================================================
    public void WriteToPageInLedger()
    {
        
        RunLedger();
        if(!LedgerImageManager.INSTANCE.IsTemporyImageNull()) //we have a temporary image which we can replace!
        {
            Debug.Log("LOG: there is a temporary that is not null, list has reached limit.");
            GameEventManager.INSTANCE?.OnEvent(typeof(ReplaceLedgerState));
            return;
        }
        
        MovePagesToFurthestLedgerImage();
        //TODO
        DrawImageManager.INSTANCE?.Drawing();
        AddImagesToLedgerPages();
      //  LedgerImageManager.INSTANCE.ReplaceImage(ledgerImagesMax, ledgerImages[ledgerImagesMax]);
        
    }
    public void MovePagesFurthestRight()
    {
         StartCoroutine(MoveRightUntilIndex(0, furthestRight, animationSpeed));
    }
    public void MovePagesToFurthestLedgerImage()
    {
         StartCoroutine(MoveRightUntilIndex(0, ledgerImages.Count - 1, animationSpeed));
    }

    public void OpenLedger()
    {
        RunLedger(); //since you opening the ledger, just run run ledger. open ledger avoids confusion. 
    }
   // ==================================================================================================================

    public void ChangeColorLayeringBorderLeft()
    {
        for(int i = 0; i < pageL; i++)
            {
                UI.LedgerUIManager.INSTANCE.ChangeLayerDown(i);
                UI.LedgerUIManager.INSTANCE.MakePageColor(i, new Color(1, 1, 1, 1));
            }  
        UI.LedgerUIManager.INSTANCE.ChangeBorderLeft();
    }

     public void ChangeColorLayeringBorderRight()
    {
        for(int i = 0; i < pageL; i++)
            {
                UI.LedgerUIManager.INSTANCE.ChangeLayerDown(i);
                UI.LedgerUIManager.INSTANCE.MakePageColor(i, new Color(1, 1, 1, 1));
            }
        UI.LedgerUIManager.INSTANCE.ChangeBorderRight();
    }
    public void ChangeColorAndLayering(int ind)
    {
        UI.LedgerUIManager.INSTANCE.NoBorder();
     
        for(int i = 0; i < pageL; i++)
        {
            if(i == ind)
            {
                UI.LedgerUIManager.INSTANCE.MakePageColor(ind, new Color(1, 0, 0, 1));
                continue;
            }
            UI.LedgerUIManager.INSTANCE.ChangeLayerDown(i);
            UI.LedgerUIManager.INSTANCE.ChangeOverLayDown(i);
            UI.LedgerUIManager.INSTANCE.MakePageColor(i, new Color(1, 1, 1, 1));
        }
         UI.LedgerUIManager.INSTANCE.ChangeLayerLeft(rotateIndex + 1, ind);   
        
    }
    public void StayIndex(int rIndex)
    {
        UI.LedgerUIManager.INSTANCE.ChangeLayerLeft(rIndex, index - 1);   
    }

    public void MovePages()
    {
        Debug.Log("current index " + index);
        if(Input.GetKeyDown(KeyCode.D))
        {
            MovePageRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MovePageLeft();
        }
    }
     public void MovePageRight()
    {
       
            LedgerData.INSTANCE.isLeft = false;
            if(index < pageL)
            {
                edgechecker = true;
                runOnce = true;
            }
            if(rotateIndex + 1 >= ledgerLength - 1)
            {
                UI.LedgerUIManager.INSTANCE.FlipPageRight(rotateIndex + 1);
                /*
                TODO continue work on the ledger layering
                Action<(int,LedgerUIManager)> a = null;
               
                a = ((int index,LedgerUIManager lm) tuple) => {
                    if(tuple.index >= ledgerLength - 1 && runOnce)
                    {
                    ChangeColorLayeringBorderRight();
                    LedgerUIManager.onAfterFlipPage.RemoveAction(a);
                    runOnce = false;
                    //LedgerUIManager.INSTANCE.onFlipAt90Degrees.RemoveAction(a);
                    }
                };
                */
               // LedgerUIManager.INSTANCE.onFlipAt90Degrees.AddAction(a); //will this cause an S.O error? lets find out!
               
                return;
            }
            ++index;
            LedgerData.INSTANCE.pageObjectsIndex = index;
            
            int indexplusone = index + 1;
           
            if(indexplusone % 2 == 0 )
            {
                UI.LedgerUIManager.INSTANCE.FlipPageRight(rotateIndex + 1);
                ++rotateIndex;
               // UI.LedgerUIManager.INSTANCE.ChangeBorderLeft();
                StayIndex(rotateIndex); 
                //and change color
                return;
            }
            ChangeColorAndLayering(index);

    }
    public void MovePageLeft()
    {
            LedgerData.INSTANCE.isLeft = true;
            if(index == 0)
            {
                ChangeColorAndLayering(0);
                ChangeColorLayeringBorderLeft();
                return;
            }
            if(index == pageL - 1 && edgechecker) //if is we are on last index and the prevous index was < 
            {
                UI.LedgerUIManager.INSTANCE.FlipPageLeft(rotateIndex + 1); 
                edgechecker = false;
               return;
            }
            if(index < pageL)
            {
                edgechecker = true;
            }

            int indexplusone = index + 1;  
             
            if(indexplusone % 2 == 0 )
            {
                 --rotateIndex;
   
                UI.LedgerUIManager.INSTANCE.FlipPageLeft(rotateIndex + 1);
                StayIndex(rotateIndex + 1); 
               // UI.LedgerUIManager.INSTANCE.ChangeBorderLeft();
                --index;
                LedgerData.INSTANCE.pageObjectsIndex = index;
                //add change color and layer

                return;
            }
            --index;
            LedgerData.INSTANCE.pageObjectsIndex = index;
           
           ChangeColorAndLayering(index);
    }

   
    //==replace functions ==================================================================================================================

 
    public void ReplacePage()
    {
       if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
       {
         //TODO do some strange animation here.
       }
       if(Input.GetKeyDown(KeyCode.Return))
       {
         LedgerImage temporaryImage = LedgerImageManager.INSTANCE.temporaryImage;
         LedgerImageManager.INSTANCE.ReplaceImage(index, temporaryImage);
         AddImagesToLedgerPages(); //reset all pages... 
         
         //TODO run "replace animation"
       } 
      
    }

     

    // ==================================================================================================================

    public void AddImagesToLedgerPages()
    {
        if(ledgerImages.Count == 0)
        {
            Debug.Log("LOG: ledger Images lis has no ledgerimage's");
            return;
        }
        for(int i = 0; i < ledgerImages.Count; i++)
        {
           UI.LedgerUIManager.INSTANCE.SetTextureToPage(i, ledgerImages[i].ledgerImage);
        }
    }



    public IEnumerator MoveRightUntilIndex(int startIndex,int toIndex, float speed)
    {
        var prevflippageS = LedgerData.INSTANCE.flipPageSpeed;
        LedgerData.INSTANCE.flipPageSpeed = animationSpeed;

        while(startIndex < toIndex)
        {
            yield return new WaitForSeconds(speed);
            MovePageRight();
            ++startIndex;
        }
        LedgerData.INSTANCE.flipPageSpeed = prevflippageS;
    }


    public void EnableLedger()
    {
        UI.LedgerUIManager.INSTANCE.OpenBook();
    }
    public void DisableLedger()
    {
        UI.LedgerUIManager.INSTANCE.CloseBook();
    }
    
   
    

    //todo create a ledger state machine...
    public void CreateLedger()
    {
       

        if (LedgerImageManager.INSTANCE.IsLedgerNull())
        {
           Debug.LogError("ledger images are null");
           //return;
        }

        for (int i = 0; i < ledgerLength; i++)
        {
            //adds page
            /*
            if(i == 0 || i == ledgerLength - 1)
            {
                UI.LedgerUIManager.INSTANCE.add
                continue;
            }
            */
            UI.LedgerUIManager.INSTANCE.AddPage(i, ledgerLength - 1);
           
        }
    }
    

   


    private void OpenLedgerState()
    {
        //add this as ledger state
        /*
        CutsceneManager.INSTANCE.SetSnapShot( new (string, Type)[] { new("LedgerState",typeof(ActiveLedgerState)),  });
        CutsceneManager.INSTANCE.SetCutSceneConditions(new (Action, bool)[] { ( () => { Debug.Log("cutscene with condition is active..."); }, isLedgerEnabled)});
        CutsceneManager.INSTANCE.PlayCutscene();
       */
       // GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState));

    }
 

}

