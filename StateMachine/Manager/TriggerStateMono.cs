using UnityEngine;
using System.Collections;
using Data;
public class TriggerStateMono : StateMono<TriggerData>
{
    private void OnEnable()
    {
        Value = TriggerData.INSTANCE;
    }
}

