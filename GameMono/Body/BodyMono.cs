using UnityEngine;
using System.Collections;

public class BodyMono : MonoBehaviour
{
    public int bodyID { get; set; }

    [SerializeField]
    BodyScriptableObject bodyScriptableObject;

    public virtual void OnEnable()
    {
        bodyScriptableObject.bodyID = bodyID;
    }
}

