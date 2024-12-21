using UnityEngine;
using System.Collections;

public class MManager : StaticInstance<MManager>
{
    //character
    public static SystemActionCall<MManager> onStartManagersAction { get; set; } = new SystemActionCall<MManager>();

    public CharacterManager characterManager { get; set; }
    public TriggerManager triggerManager { get; set; }
    public DialogueManager dialogueManager { get; set; }
    public DialogueActionManager dialogueActionManager { get; set; }
    public QuestionResponseManager questionResponseManager { get; set; }
    public UIManager uIManager { get; set; }
    public MovementManager movementManager { get; set; }


    public void StartManagers()
    {
        onStartManagersAction.RunAction(this); //todo subsribe all manager with

        characterManager?.m_Start();

        triggerManager?.m_Start(); //empty

        dialogueManager?.m_Start(); //empty

        questionResponseManager?.m_Start();

        dialogueActionManager?.m_Start();

        uIManager?.m_Start();

        movementManager?.m_Start();

        //runs all other that are not defined.

    }
    private void Start()
    {
        StartManagers();
    }
}

