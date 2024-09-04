using UnityEngine;
using System.Collections;
using Data;
public class PlayerDataMono : StateMono<PlayerData>
{
    private void OnEnable()
    {
        Value = PlayerData.INSTANCE;
    }
}

