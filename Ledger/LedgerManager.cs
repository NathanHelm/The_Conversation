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
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Explorer;

public class LedgerManager : StaticInstance<LedgerManager>
{
    public static SystemActionCall<LedgerManager> onStartLedgerData = new SystemActionCall<LedgerManager>();
    public static SystemActionCall<LedgerManager> onActiveLedger = new SystemActionCall<LedgerManager>();
    
    public static SystemActionCall<LedgerManager> onMovePageLeft = new SystemActionCall<LedgerManager>();
    public static SystemActionCall<LedgerManager> onMovePageRight = new SystemActionCall<LedgerManager>();
    
    public static SystemActionCall<LedgerManager> onAfterMovePageFurthestLeft = new SystemActionCall<LedgerManager>();

    public static SystemActionCall<LedgerManager> onSelectPage = new SystemActionCall<LedgerManager>();

    public List<LedgerImage> ledgerImages = new List<LedgerImage>();
    
    public int index{get; set;} = 0;
    int rotateIndex = 0;

    public readonly int ledgerLength = 10;


    public bool isLedgerCreated {get;set;} = false;

    public bool isLedgerActive {get;set;} = false;
    
    private int pageObjectLength; 

    int furthestRight; //index of furthest-most ledger image

    public bool edgechecker {get; private set;} = true;

    public bool isLeft {get; set;} = true;

    [SerializeField]
    float speed = 2.7f; 
    float animationSpeed;

    bool runOnce = true;


  

    public override void OnEnable()
    {
       // Debug.LogError("OI bruv its time you lock tf in and get pages rights for ledger manager.");
        MManager.onStartManagersAction?.AddAction((MManager m) => { m.ledgerManager = this; });
        base.OnEnable();
    }


    public override void m_Start()
    {
        LedgerUIManager.onFlipAt90Degrees.AddPriorityAction(((bool,int,LedgerUIManager) ledgerManagerUI) => { 
            ChangeLayering(ledgerManagerUI.Item2) ;});
        LedgerUIManager.onAfterFlipPage.AddPriorityAction(((bool, int,LedgerUIManager) ledgerManagerUI) => {  
            ChangeLayeringAfterPageFlip(ledgerManagerUI.Item2); });

        LedgerUIManager.onBeginFlipPage.AddPriorityAction(((bool, int,LedgerUIManager) ledgerManagerUI) => {  
            ChangeLayeringOnBeginFlipPage(ledgerManagerUI.Item2);});

        
        base.m_Start();
        onStartLedgerData.RunAction(this);
         
    }
    public void UseLedgerState()
    {
        //Pressing tab opens the ledger
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Tab))
        {
            if (CutsceneData.INSTANCE != null)
            {
                CutsceneManager.INSTANCE?.LedgerDialog();
            }
            else
            {
                Debug.LogError("cutscene data not found");
            }
            GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState));       
        }

    }
    public void ResetLedger()
    {
        MovePagesToFurthestLeft();
    }
    private void RunLedger()
    {
       

        if(!isLedgerCreated)
        {
            CreateLedger(); //make a ten page ledger
            LedgerData.INSTANCE.isLedgerCreated = isLedgerCreated = true;

            pageObjectLength = UI.LedgerUIManager.INSTANCE.GetPageLength();
            animationSpeed = (speed / pageObjectLength);
            furthestRight = 2 * (ledgerLength - 2);


            LedgerData.INSTANCE.isLeft = false;
            UI.LedgerUIManager.INSTANCE.FlipPageRight(0,0);


            ChangeColorLayeringBorderLeft();
        }
        if (!isLedgerActive)
        {
            LedgerMovement.INSTANCE.MoveHandLeft();
            EnableLedger();
            GameEventManager.INSTANCE.OnEvent(typeof(PlayCutsceneState));
            isLedgerActive = true;
        

        }

        
        onActiveLedger.RunAction(this); //for one, upadte ledgerimages list...
        //AddImagesToLedgerPages();

       
      

      
 
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

        //move furthest right begins AFTER pages are at index 0 and have moved left!

        onAfterMovePageFurthestLeft.AddAction(lm => { 

        MovePagesToFurthestLedgerImage();  

        LedgerMovement.onAfterFlipAwait.AddAction(
        LedgerData.INSTANCE.writeActionLedgerMovement
        );
        
        });
        ResetLedger();
        

   
        DrawingManager.INSTANCE?.RunDrawingPPEffect();
        AddImagesToLedgerPages();
      //  LedgerImageManager.INSTANCE.ReplaceImage(ledgerImagesMax, ledgerImages[ledgerImagesMax]);
        
    }
    public void MovePagesFurthestRight()
    {
         StartCoroutine(MoveRightUntilIndex(index, furthestRight, animationSpeed, 0));
    }
    public void MovePagesToFurthestLedgerImage()
    {
        StartCoroutine(MoveRightUntilIndex(0, ledgerImages.Count - 1, animationSpeed, 0f));
    }
    public void MovePagesToFurthestLeft()
    {
        StartCoroutine(MovePageLeftUntilIndex(index, 0, animationSpeed, 0));
      
    }

    public void OpenLedger()
    {
        RunLedger(); //since you opening the ledger, just run run ledger. open ledger avoids confusion. 
    }
   // ==================================================================================================================

    
    public void ChangeColorLayeringBorderLeft()
    {
        for(int i = 0; i < pageObjectLength; i++)
            {
                UI.LedgerUIManager.INSTANCE.LayerDownAtIndex(i, 2500);
            }  
        UI.LedgerUIManager.INSTANCE.ChangeBorderLeft();
    }

     public void ChangeColorLayeringBorderRight()
    {
        //TODO
        Debug.LogError("add change color layering border right...");

        for(int i = 0; i < pageObjectLength; i++)
        {
                UI.LedgerUIManager.INSTANCE.LayerDownAtIndex(i, 2500);
        }
        UI.LedgerUIManager.INSTANCE.ChangeBorderRight();

    }
    public void ChangeLayering(int ind)
    {
         UI.LedgerUIManager.INSTANCE.ChangeLayerUp(ind, 3000);
         UI.LedgerUIManager.INSTANCE.ChangeLayerDown(ind, 2700, 2600);
       
    }
    public void ChangeLayeringOnBeginFlipPage(int ind)
    {
        UI.LedgerUIManager.INSTANCE.NoBorder();
        UI.LedgerUIManager.INSTANCE.ChangeLayerUp(ind, 2900);
    }
    public void ChangeLayeringAfterPageFlip(int ind)
    {
        UI.LedgerUIManager.INSTANCE.ChangeLayerDown(ind, 2500, 2500);
    }

    public void MovePages()
    {
        // Debug.Log("current index " + index);
        if (Input.GetKeyDown(KeyCode.D))
        {
            MovePageRight();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MovePageLeft();
        }
    }
    public void SelectPage()
    {
        if(InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return))
        {
            //select the page
            onSelectPage.RunAction(this);
        }
    }
     public void MovePageRight()
    {
       
            LedgerData.INSTANCE.isLeft = false;

            if(index < pageObjectLength)
            {
                edgechecker = true;
                runOnce = true;
            }
            if(rotateIndex + 1 >= ledgerLength - 1)
            {
                UI.LedgerUIManager.INSTANCE.FlipPageRight(index,rotateIndex + 1);
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
                UI.LedgerUIManager.INSTANCE.FlipPageRight(index,rotateIndex + 1);
                ++rotateIndex;
               // UI.LedgerUIManager.INSTANCE.ChangeBorderLeft();
           
                //and change color
                return;
            }
           // GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
            onMovePageRight.RunAction(this);
           // ChangeLayering()

    }
    public void MovePageLeft()
    {
            LedgerData.INSTANCE.isLeft = true;
     
            if(index == 0)
            {
                //ChangeLayering(0);
                //ChangeColorLayeringBorderLeft();
                return;
            }
            if(index == pageObjectLength - 1 && edgechecker) //if is we are on last index and the prevous index was < 
            {
                UI.LedgerUIManager.INSTANCE.FlipPageLeft(index - 1,rotateIndex + 1); 
                edgechecker = false;
               return;
            }
            if(index < pageObjectLength)
            {
                edgechecker = true;
            }

            int indexplusone = index + 1;  
             
            if(indexplusone % 2 == 0 )
            {
                 --rotateIndex;
   
                UI.LedgerUIManager.INSTANCE.FlipPageLeft(index - 1,rotateIndex + 1);
            
               // UI.LedgerUIManager.INSTANCE.ChangeBorderLeft();
                --index;
                LedgerData.INSTANCE.pageObjectsIndex = index;
                //add change color and layer

                return;
            }
            --index;
            LedgerData.INSTANCE.pageObjectsIndex = index;
           
           onMovePageLeft.RunAction(this);
          // GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
          // ChangeColorAndLayering(index);
    }

   
    //==replace functions ==================================================================================================================

 
    public void ReplacePage()
    {
       if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
       {
         //TODO do some strange animation here.
       }
       if(InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return))
       {
         LedgerImage temporaryImage = LedgerImageManager.INSTANCE.temporaryImage;
         LedgerImageManager.INSTANCE.ReplaceImage(index, temporaryImage);
         AddImagesToLedgerPages(); //reset all pages... 
         LedgerImageManager.INSTANCE.SetTemporaryImageToNull();
        //disable left hand after the erase animation:
         PageAnimations.onAfterEraseImage.AddAction(LedgerData.INSTANCE.disableleftHandPage);
         HandAnimations.INSTANCE.EraseImageAnimationLeftHandPage();

        //page that is being replaced get an animation
        PageAnimations.INSTANCE.DrawImageOnCurrentPage();

        GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState));
         
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
    public void SetImageValueToOne(int index)
    {

        Renderer ren = LedgerUIManager.INSTANCE.GetPageOverlayRenderer(index);
        ren.material.SetFloat("_Val", 1);

    }



    public IEnumerator MoveRightUntilIndex(int startIndex, int toIndex, float speed, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        var prevflippageS = LedgerData.INSTANCE.flipPageTime;
        LedgerData.INSTANCE.flipPageTime = animationSpeed + 0.5f;



        while (startIndex < toIndex)
        {

            yield return new WaitForSeconds(speed);
            MovePageRight();
            ++startIndex;
        }
        if (index == 0)
        {
            LedgerMovement.INSTANCE.MoveHandAwaitPoint();
        }

        LedgerData.INSTANCE.flipPageTime = prevflippageS;
        yield return null;
    }
    public IEnumerator MovePageLeftUntilIndex(int startIndex,int toIndex, float speed, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        var prevflippageS = LedgerData.INSTANCE.flipPageTime;
        LedgerData.INSTANCE.flipPageTime = animationSpeed + 0.5f;

        while(startIndex >= toIndex)
        {
            yield return new WaitForSeconds(speed);
            MovePageLeft();
            --startIndex;
        }
        if(index == 0)
        {
           LedgerMovement.INSTANCE.MoveHandAwaitPoint();
        }

        LedgerData.INSTANCE.flipPageTime = prevflippageS;

        onAfterMovePageFurthestLeft.RunAction(this);
        onAfterMovePageFurthestLeft.RemoveAllActions();
        yield return null;
    }


    public void EnableLedger()
    {
        UI.LedgerUIManager.INSTANCE.OpenBook();

    }
    public void DisableLedger()
    {
       
        isLedgerActive = false;
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

