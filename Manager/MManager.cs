using UnityEngine;
using System.Collections;

public class MManager : StaticInstance<MManager>
{
    //character
    CharacterManager characterManager;

    //dialog
    DialogueManager dialogueManager;
    DialogueActionManager dialogueActionManager;
    QuestionResponseManager questionResponseManager;

    //trigger
    TriggerManager triggerManager;
    TriggerActionManager triggerActionManager;

    //UI
    UIManager uIManager;

    //movement
    MovementManager movementManager;


    private void SetManagers()
    {
        characterManager = GameEventManager.INSTANCE.OnEventFunc<CharacterManager>("charactermanager");
        dialogueManager = GameEventManager.INSTANCE.OnEventFunc<DialogueManager>("dialoguemanager");
        dialogueActionManager = GameEventManager.INSTANCE.OnEventFunc<DialogueActionManager>("dialogueactionmanager");
        questionResponseManager = GameEventManager.INSTANCE.OnEventFunc<QuestionResponseManager>("questionresponsemanager");
        triggerManager = GameEventManager.INSTANCE.OnEventFunc<TriggerManager>("triggermanager");
        triggerActionManager = GameEventManager.INSTANCE.OnEventFunc<TriggerActionManager>("triggeractionmanager");
        uIManager = GameEventManager.INSTANCE.OnEventFunc<UIManager>("uimanager");
        movementManager = GameEventManager.INSTANCE.OnEventFunc<MovementManager>("movementmanager");
    }
    public void StartManagers()
    {
        SetManagers();

        characterManager.m_Start();

        triggerManager.m_Start(); //empty

        dialogueManager.m_Start(); //empty

        questionResponseManager.m_Start();

        dialogueActionManager.m_Start();

        uIManager.m_Start();

        movementManager.m_Start();



    }
    private void Start()
    {
        StartManagers();
    }
}

