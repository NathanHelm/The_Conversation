using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSelectionManager : StaticInstance<DialogueSelectionManager>, IExecution
{
    [SerializeField]
    private UIScriptableObject uIScriptableObject;
    private GameObject buttonPrefab;
    private CharacterMono[] characterMonosInTrigger; //dialogue!

    private List<GameObject> spawnedUIObject = new();

    private int index;

    private float width;
    private float height;

    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction((MManager manager) => { manager.dialogueSelectionManager = this; });
    }

    public void SetUpDialogueSelectionManager()
    {
        buttonPrefab = uIScriptableObject.dialogueSelectionObject.uiDialogueButtonImage;
        width = uIScriptableObject.dialogueSelectionObject.width;
        height = uIScriptableObject.dialogueSelectionObject.height;

    }
    public override void m_Start()
    {
        SetUpDialogueSelectionManager();
    }

    public void CreateButtons()
    {
        characterMonosInTrigger = Data.TriggerData.INSTANCE.characterMonosInTrigger.ToArray();
        int j = 0;
        for (int i = 0; i < characterMonosInTrigger.Length; i++)
        {
            GameObject g = null;
            spawnedUIObject.Add(g = Instantiate(buttonPrefab, GameObject.FindGameObjectWithTag("ButtonParent").transform));
            g.GetComponent<RectTransform>().localPosition = new Vector3(g.GetComponent<RectTransform>().anchoredPosition.x * (i % 3) + width, -(j * height), 1);

            if (i > 0 && i % 3 == 0)
            {
                j++;
                g.GetComponent<RectTransform>().localPosition = new Vector3(g.GetComponent<RectTransform>().anchoredPosition.x * (i % 3) + width, -(j * height), 1);

            }
        }
        //create a button
        //three column grid of button based on amount of objects in trigger
    }
    public void SetTextToName()
    {
        for (int i = 0; i < spawnedUIObject.Count; i++)
        {
            var dialoguebuttonText = spawnedUIObject[i].GetComponentInChildren<TextMeshProUGUI>();
            dialoguebuttonText.text = characterMonosInTrigger[i].bodyName;
        }
    }
    public void ButtonIndexUp()
    {
        ++index;
        index = Mathf.Min(spawnedUIObject.Count - 1, index);
    }
    public void ButtonIndexDown()
    {
        --index;
       index = Mathf.Max(0, index);
    }
    private void ShowButton()
    {
        spawnedUIObject[index].GetComponent<Image>().color = new UnityEngine.Color(.7f,.7f,.7f,1);
    }
    private void HideButton()
    {
        spawnedUIObject[index].GetComponent<Image>().color = new UnityEngine.Color(1,1,1,1);
    }
    private void RunConversationTrigger()
    {
        var dialogueData = Data.DialogueData.INSTANCE;
        int characterId = characterMonosInTrigger[index].bodyID;
        int questionId = characterMonosInTrigger[index].persistentConversationQuestionId; //change persistent conversation Id

        dialogueData.currentCharacterID = characterId;
        dialogueData.currentQuestionID = questionId;
        //TODO what does this do? dialogueData.currentPersistentConversationID = trigger.charactersOnTrigger[0].persistentConversationId;
        TriggerActionManager.INSTANCE.PlayTriggerAction(2001);
    }
    public void StartSelectionButtonLeft()
    {
        index = 0;
        ShowButton();
        
    }
    private void DestoryButtons()
    {
        int last = spawnedUIObject.Count - 1;
        while (last >= 0)
        {
            Destroy(spawnedUIObject[last]);
            --last;
        }
        spawnedUIObject.Clear();
    }
    public void RunSelectedDialogue()
    {
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.A))
        {
            HideButton();
            ButtonIndexUp();
            ShowButton();
        }
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.D))
        {
            HideButton();
            ButtonIndexDown();
            ShowButton();
        }
        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return))
        {
            DestoryButtons();
            RunConversationTrigger();
        }
    }

}