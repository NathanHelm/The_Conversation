
using UnityEngine;
using System.Collections;
using UI;
using System.Buffers;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class MManager : StaticInstance<MManager>, IExecution
{
    //character
    public SystemActionCall<MManager> onStartManagersAction { get; set; } = new SystemActionCall<MManager>();

    public SavePersistenceManager savePersistenceManager { get; set; } = null;
    public CharacterManager characterManager { get; set; } = null;
    public TriggerActionManager triggerActionManager { get; set; } = null;
    public TriggerManager triggerManager { get; set; } = null;
    //================================================================================================================================================
    public DialogueManager dialogueManager { get; set; } = null;
    public DialogueActionManager dialogueActionManager { get; set; } = null;

    public DialogueSelectionManager dialogueSelectionManager { get; set; } = null;
    //================================================================================================================================================
    public QuestionResponseManager questionResponseManager { get; set; } = null; 
    public UIManager uIManager { get; set; } = null;

    public ActionController actionController { get; set; } = null;
    public ButtonDialogueManager buttonDialogueManager { get; set; } = null;
    public MovementManager movementManager { get; set; } = null;

//================================================================================================================================================================
    public LedgerManager ledgerManager { get; set; } = null;
    public LedgerUIManager ledgerUIManager { get; set; } = null;
    public LedgerImageManager ledgerImageManager {get; set;} = null;
    public LedgerMovement ledgerMovement {get; set;} = null;

    public HandAnimations handAnimations {get; set;} = null;

    public ImageUIAnimations pageAnimations {get; set;} = null;
    
    public IconUIAnimations imageUIAnimations { get; set;} = null;

    public DrawingManager drawingManager { get; set; } = null;

//================================================================================================================================================================
    public AnimationManager animationManager { get; set; } = null;
    public CutsceneManager cutsceneManager { get; set; } = null;
    
    public SceneManager sceneManager { get; set; } = null;


    public MemoryManager memoryManager { get; set; } = null;
    public MemorySpawnerManager memorySpawnerManager { get; set; } = null;

    public TransitionManager transitionManager { get; set; } = null;

    public ClueCameraManager clueCameraManager { get; set; } = null;

    public StateManager stateManager { get; set; } = null; //this one stays last

    public void StartData()
    {}
    public void StartManagers()
    {
        onStartManagersAction.RunAction(this); //todo subsribe all manager with

        savePersistenceManager?.m_Start();

        memoryManager?.m_Start();

        memorySpawnerManager?.m_Start();

        characterManager?.m_Start();

        triggerActionManager?.m_Start();

        triggerManager?.m_Start(); //empty

        dialogueManager?.m_Start(); //empty

        dialogueSelectionManager?.m_Start();

        questionResponseManager?.m_Start();

        dialogueActionManager?.m_Start();

        uIManager?.m_Start();

        buttonDialogueManager?.m_Start();

        clueCameraManager?.m_Start();

        movementManager?.m_Start();

       

        ledgerManager?.m_Start();

        ledgerUIManager?.m_Start();

        ledgerImageManager?.m_Start();

        animationManager?.m_Start();

        ledgerMovement?.m_Start();

        handAnimations?.m_Start();

        pageAnimations?.m_Start();

        imageUIAnimations?.m_Start();

        drawingManager?.m_Start();

        cutsceneManager?.m_Start();

        transitionManager?.m_Start();

        actionController?.m_Start();

        stateManager?.m_Start(); //state manager ALWAYS comes last. 

        sceneManager?.m_Start(); //...unless your scene makes changes which will change state. 

        Debug.Log("LOG: managers ran their m_Start!");


        //runs all other that are not defined.

    }
    public void Start()
    {
        StartManagers();
 
    }
}

