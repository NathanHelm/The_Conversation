using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
public class InputBuffer : MonoBehaviour{

    public static InputBuffer INSTANCE;
    private void Awake()
    {
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
    }

    private bool isInputActive = false;
    private IEnumerator singleCoroutine;
    public bool IsInputChecked()
    {
        if(isInputActive)
        {
           return true;
        }
        return false;
        
    }
    public bool IsPressCharacter(KeyCode keyCode)
    {
        //code will only run once per frame
        var currentInput = Input.GetKeyDown(keyCode);
        if(currentInput && !IsInputChecked())
        {
            isInputActive = currentInput;
            if(singleCoroutine == null)
            {
               StartCoroutine(singleCoroutine = InputValue());
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }
    private IEnumerator InputValue()
    {
        yield return new WaitForEndOfFrame();
        isInputActive = false;
        singleCoroutine = null;

        yield return null;
    }
    

}