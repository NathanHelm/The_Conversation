using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DialogueButton : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler{

    public int ID {get; set;} 
    private bool isOnPointer = false;
   public void OnPointerEnter(PointerEventData eventData)
    {
        /*
        if(ButtonDialogueManager.INSTANCE != null)
        {
            if(ID == 1)
            {

            ButtonDialogueManager.INSTANCE.ButtonYesColorYellow();
            ButtonDialogueManager.INSTANCE.ButtonNoColorWhite();
            ButtonDialogueManager.INSTANCE.SetIsYes(true);

            }
            if(ID == 2)
            {

            ButtonDialogueManager.INSTANCE.ButtonNoColorYellow();
            ButtonDialogueManager.INSTANCE.ButtonYesColorWhite();
            ButtonDialogueManager.INSTANCE.SetIsYes(false);
            }
            Debug.Log("ON!");
            
        }
        */
    }

          
    

    public void OnPointerExit(PointerEventData eventData)
    {
        /*
        if(ButtonDialogueManager.INSTANCE != null)
        {
            ButtonDialogueManager.INSTANCE.ButtonYesColorYellow();
            ButtonDialogueManager.INSTANCE.ButtonNoColorWhite();
            ButtonDialogueManager.INSTANCE.SetIsYes(true);
            
        }
       Debug.Log("OFF!");
       */
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }

    
}
