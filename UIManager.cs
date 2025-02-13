
using UnityEngine;
using System.Collections;
using Data;
using TMPro;
using System;

public class UIManager : StaticInstance<UIManager>
{
    

    TextMeshProUGUI dialogText;

    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction((MManager m) =>
        {
            m.uIManager = this;
        });
    }

    public override void m_Start()
    {
       
        if (UIData.INSTANCE != null)
        {
            dialogText = UIData.INSTANCE.dialogText;
        }
        else
        {
            throw new Exception("UI data is null");
        }
        //communcating to dialog manager, setting UI manager.
    }

    private void UIFogIn()
    {
      // UIData.
    }

    public void ResetText()
    {
        if(dialogText == null)
        {
            throw new NullReferenceException("dialog is not added");
        }
        dialogText.text = "";
    }
    public TextMeshProUGUI GetDialogText()
    {
        return dialogText;
    }

    public void EnableDialogUI()
    {
        UIData.INSTANCE.dialogBlock.enabled = true;
        UIFogIn();
        //todo show character
    }
    public void DisableDialogUI()
    {
        if (UIData.INSTANCE.dialogText != null)
        {
            UIData.INSTANCE.dialogText.text = "";
            UIData.INSTANCE.dialogBlock.enabled = false;
        }
    }

    public void EnableUIObject(ref GameObject uiObject)
    {
        uiObject.SetActive(true);
    }
    public void DisableUIObject(ref GameObject uiObject)
    {
        uiObject.SetActive(false);
    }
    public void ChangePosition(ref GameObject uiObject, Vector3 uiObjectPos)
    {
        var rectTransform = uiObject.GetComponent<RectTransform>();
        rectTransform.localPosition = uiObjectPos;
    }
 
    
}

