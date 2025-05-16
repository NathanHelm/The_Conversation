
using UnityEngine;
using System.Collections;
using UI;
using System.Buffers;
using Unity.Mathematics;

public class MManager : StaticInstance<MManager>
{
    //character
    public static SystemActionCall<MManager> onStartManagersAction { get; set; } = new SystemActionCall<MManager>();

    public SavePersistenceManager savePersistenceManager {get; set;}
    public CharacterManager characterManager { get; set; }
    public TriggerActionManager triggerActionManager { get; set; }
    public TriggerManager triggerManager { get; set; }
    public DialogueManager dialogueManager { get; set; }
    public DialogueActionManager dialogueActionManager { get; set; }
    public QuestionResponseManager questionResponseManager { get; set; }
    public UIManager uIManager { get; set; }

    public ButtonDialogueManager buttonDialogueManager {get; set;}
    public MovementManager movementManager { get; set; }

//================================================================================================================================================================
    public LedgerManager ledgerManager { get; set; }
    public LedgerUIManager ledgerUIManager { get; set; }
    public LedgerImageManager ledgerImageManager {get; set;}
    public LedgerMovement ledgerMovement {get; set;}

    public HandAnimations handAnimations {get; set;}

    public PageAnimations pageAnimations {get; set;}

    public DrawingManager drawingManager {get; set;}

//================================================================================================================================================================
    public AnimationManager animationManager {get; set;}
    public CutsceneManager cutsceneManager { get; set; }
    public MemoryManager memoryManager {get; set;}



    public TransitionManager transitionManager {get; set;}

    public StateManager stateManager {get; set;} //this one stays last


    public void StartManagers()
    {
        onStartManagersAction.RunAction(this); //todo subsribe all manager with

        savePersistenceManager?.m_Start();
        
        characterManager?.m_Start();

        triggerActionManager?.m_Start();

        triggerManager?.m_Start(); //empty

        dialogueManager?.m_Start(); //empty

        questionResponseManager?.m_Start();

        dialogueActionManager?.m_Start();

        uIManager?.m_Start();

        buttonDialogueManager?.m_Start();

        movementManager?.m_Start();

        ledgerManager?.m_Start();

        ledgerUIManager?.m_Start();

        ledgerImageManager?.m_Start();

        animationManager?.m_Start();

        ledgerMovement?.m_Start();

        handAnimations?.m_Start();

        pageAnimations?.m_Start();

        drawingManager?.m_Start();

        cutsceneManager?.m_Start();

        transitionManager?.m_Start();

        memoryManager?.m_Start();

        stateManager?.m_Start(); //state manager ALWAYS comes last. 

       
        Debug.Log("LOG: managers ran their m_Start!");


        //runs all other that are not defined.

    }
    public void Start()
    {
        StartManagers();
    }
}

