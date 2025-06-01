using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInstanceData<T> : StaticInstance<T> where T : MonoBehaviour
{
    public override void OnDisable()
    {
        Debug.Log("This is a disable object");
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }
    
}