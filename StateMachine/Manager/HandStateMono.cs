using UnityEngine;
using System.Collections;
using Data;
public class HandStateMono : StateMono<LedgerData>
{
    public void OnEnable()
    {
        Value = LedgerData.INSTANCE;
    }

}

