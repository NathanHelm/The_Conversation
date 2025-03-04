using UnityEngine;
using UnityEngine.UI;
public class DialogueButton : MonoBehaviour{

    public Button b {get; set;}
    void Awake()
    {
        b = GetComponent<Button>();
    }

}
