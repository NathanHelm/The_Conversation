using UnityEngine;
using System.Collections;
using Data;
public class CutsceneMono :  StateMono<CutsceneData>
{
    public void OnEnable()
    {
        Value = CutsceneData.INSTANCE;
    }
}

