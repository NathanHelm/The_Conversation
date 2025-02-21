using UnityEngine;
using System.Collections;
using System.Data;
using Data;

public class LedgerStateMono : StateMono<Data.LedgerData>
{
	private void OnEnable()
    {
        Value = LedgerData.INSTANCE;
    }
}

