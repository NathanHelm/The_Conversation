using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
[System.Serializable]
public class DialogueObject
{

    [SerializeField]
    [TextArea(5, 5)]
    public string line; //DO NOT TOUCH
    [SerializeField]
    public Sprite image;
    [SerializeField]
    public Texture imageTex;

}

