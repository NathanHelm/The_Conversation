﻿using UnityEngine;
using System.Collections;
using Data;
using TMPro;
using System;

public class UIManager : StaticInstance<UIManager>
{
    UIData uIData;

    TextMeshProUGUI dialogText;

    public override void m_Start()
    {
        uIData = GameEventManager.INSTANCE.OnEventFunc<UIData>("data.uidata");
        if (uIData != null)
        {
            dialogText = uIData.dialogText;
        }
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
        uIData.dialogBlock.enabled = true;
        UIFogIn();
        //todo show character
    }
    public void DisableDialogUI()
    {
        uIData.dialogText.text = "";
        uIData.dialogBlock.enabled = false;


    }
}

