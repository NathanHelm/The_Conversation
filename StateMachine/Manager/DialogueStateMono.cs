using UnityEngine;
using System.Collections;
using Data;
public class DialogueStateMono : StateMono<DialogueData>
{
    private void OnEnable()
    {
        Value = DialogueData.INSTANCE;
    }
}

