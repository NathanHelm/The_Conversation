using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonDialogueManager : StaticInstance<ButtonDialogueManager>, IPointerEnterHandler, IPointerExitHandler{

private Action action1, action2;

public Button button1, button2;

private  bool isYes = false;

    public override void m_Start()
    {
       actionCall.RunAction(this);
    }

    public void SetActions(Action action1 , Action action2){
    this.action1 = action1; this.action2 = action2;
}
public void SetButtonNames(string forYes, string forNo)
{
    button1.GetComponent<TextMeshProUGUI>().text = forYes;
    button2.GetComponent<TextMeshProUGUI>().text = forNo;
}
public void EnableButtons()
{
    button1.enabled = true;
    button2.enabled = true;
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
    button1.enabled = false;
    button2.enabled = false;
}
private void ButtonYesColorYellow()
{
    button1.GetComponent<Image>().color = Color.yellow + new Color(0.1f, 0.1f, 0.1f, 1.0f);
    button2.GetComponent<Image>().color = Color.white;
}
private void ButtonYesColorWhite()
{
    button2.GetComponent<Image>().color = Color.yellow + new Color(0.1f, 0.1f, 0.1f, 1.0f);
    button1.GetComponent<Image>().color = Color.white;
}
    public void BetweenButtons()
{
    if(Input.GetKeyDown(KeyCode.A))
    {
       ButtonYesColorYellow();
       isYes = true;
    }
    if(Input.GetKeyDown(KeyCode.B))
    {
       ButtonYesColorWhite();
        isYes = false;
    }

    if(Input.GetKeyDown(KeyCode.Return))
    {
      if(isYes)
      {
        button1.Select();
        return;
      }
        button2.Select();
      
    }
}

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonYesColorYellow();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonYesColorWhite();
    }
}