using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonDialogueManager : StaticInstance<ButtonDialogueManager>, IExecution{

public static SystemActionCall<ButtonDialogueManager> onActionStart = new SystemActionCall<ButtonDialogueManager>();
private Action action1, action2;

public Button button1, button2;

private  bool isYes = false;

    public override void m_OnEnable()
    {
       MManager.INSTANCE.onStartManagersAction.AddAction((MManager e) => {e.buttonDialogueManager = this;});
    }

    public override void m_Start()
    {
       onActionStart.RunAction(this);
       ButtonNoColorYellow();
       ButtonYesColorWhite();
       HideButtons();
    }

    public void SetActions(Action action1 , Action action2){
    
    this.action1 = action1; this.action2 = action2;
    
    }
public void SetButtonNames(string forYes, string forNo)
{
    button1.GetComponent<TextMeshProUGUI>().text = forYes;
    button2.GetComponent<TextMeshProUGUI>().text = forNo;
}

public void AddEventsToButtons()
{
    if(action1 == null|| action2 == null)
    {
        Debug.LogError("actions are null, set action before setting button state.");
        return;
    }
    button1.onClick.AddListener(()=>{action1();});
    button2.onClick.AddListener(()=>{action2();});
}
public void RemoveEventsToButtons()
{
    button1.onClick.RemoveAllListeners();
    button2.onClick.RemoveAllListeners();
}


public void HideButtons()
{
    button1.gameObject.SetActive(false);
    button2.gameObject.SetActive(false);
}
public void EnableButtons()
{
    button1.gameObject.SetActive(true);
    button2.gameObject.SetActive(true);
}

//button color transitions.
public void ButtonYesColorYellow()
{
    button1.GetComponent<Image>().color = Color.yellow - new Color(0.1f, 0.1f, 0.1f, 0f);
}
public void ButtonNoColorYellow()
{
    button2.GetComponent<Image>().color = Color.yellow - new Color(0.1f, 0.1f, 0.1f, 0f);

}
public void ButtonYesColorWhite()
{
    button1.GetComponent<Image>().color = Color.white;
   
}
public void ButtonNoColorWhite()
{
     button2.GetComponent<Image>().color = Color.white;
}

public void SetIsYes(bool isYes)
{
    this.isYes = isYes;
}
public void Select()
{
    if(isYes)
    {
        button1.Select();
        return;
    }
        button2.Select();
}

public void BetweenButtons()
{
    if(Input.GetKeyDown(KeyCode.A))
    {
      
        ButtonYesColorYellow();
        ButtonNoColorWhite();
        isYes = true;
    }
    if(Input.GetKeyDown(KeyCode.D))
    {
      
       ButtonNoColorYellow();
       ButtonYesColorWhite();
       isYes = false;
    }

    if(InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Return))
    {
      Select();
      return;
    }
}
public void R()
{
    StartCoroutine(waitjustalittle());
}

public IEnumerator waitjustalittle()
{
    yield return new WaitForSeconds(0.1f);
}

}