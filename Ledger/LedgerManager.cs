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

    int index = 0;
    int rotateIndex = 0;

    public readonly int ledgerLength = 10;

    public bool isLedgerCreated = true;
    
    int pageL; 

    bool edgechecker = true;

    [SerializeField]
    float speed = 2.7f; 
    float animationSpeed;


  

    public override void OnEnable()
    {
       // Debug.LogError("OI bruv its time you lock tf in and get pages rights for ledger manager.");
     //   MManager.onStartManagersAction?.AddAction((MManager m) => { m.ledgerManager = this; });
        base.OnEnable();
    }


    public override void m_Start()
    {
        
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

        animationSpeed = speed / pageL;
        UI.LedgerUIManager.INSTANCE.FlipPageRight(0);
        ChangeColorAndLayering(index);
 
    }
    //==run on state start ==================================================================================================================
    public void WriteToPageInLedger()
    {
        
        RunLedger();
        if(LedgerImageManager.INSTANCE.IsLedgerNull())
        {
              GameEventManager.INSTANCE.OnEvent(typeof(ReplaceLedgerState));
              return;
        }
        //TODO make 15 a desired variable... 
        
        MovePagesFurthestRight();
        //TODO
        DrawImageManager.INSTANCE.Drawing();
        
    }
    public void MovePagesFurthestRight()
    {
         StartCoroutine(MoveRightUntilIndex(0, 15, animationSpeed));
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
                UI.LedgerUIManager.INSTANCE.ChangeLayerLeft(ind);
                continue;
            }
            UI.LedgerUIManager.INSTANCE.ChangeLayerDown(i);
            UI.LedgerUIManager.INSTANCE.MakePageColor(i, new Color(1, 1, 1, 1));
        }   
        
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

            if(rotateIndex + 1 >= ledgerLength - 1)
            {
                UI.LedgerUIManager.INSTANCE.FlipPageRight(rotateIndex + 1);
                ChangeColorLayeringBorderRight();
                return;
            }
            ++index;
            
            int indexplusone = index + 1;
           
            if(indexplusone % 2 == 0 )
            {
                
                UI.LedgerUIManager.INSTANCE.FlipPageRight(rotateIndex + 1);
                ++rotateIndex;
            }
            ChangeColorAndLayering(index);
    }
    public void MovePageLeft()
    {
            if(index == 0)
            {
                ChangeColorAndLayering(0);
                return;
            }
            if(index == pageL - 1 && edgechecker)
            {
                UI.LedgerUIManager.INSTANCE.FlipPageLeft(rotateIndex + 1); 
                edgechecker = false;
                ChangeColorAndLayering(index);
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
               
            }
            --index;
           
           ChangeColorAndLayering(index);
    }

   
   
    public void ReplacePageToLedger()
    {
        MovePagesFurthestRight();
       if(Input.GetKeyDown(KeyCode.Return))
       {
         LedgerImage temporaryImage = LedgerImageManager.INSTANCE.temporaryImage;
         LedgerImageManager.INSTANCE.ReplaceImage(index, temporaryImage);
       } 
    }

 

    public IEnumerator MoveRightUntilIndex(int startIndex,int toIndex, float speed)
    {
        LedgerData.INSTANCE.flipPageSpeed = animationSpeed;
        while(startIndex < toIndex)
        {
            yield return new WaitForSeconds(speed);
            MovePageRight();
            ++startIndex;
        }
        LedgerData.INSTANCE.flipPageSpeed = 1f;
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

