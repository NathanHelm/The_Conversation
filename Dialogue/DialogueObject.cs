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
     [Header("we might not need this--1")]
    public Sprite image;
    [SerializeField]
    [Header("we might not need this--2")]
    public Texture imageTex;

    [SerializeField]
    public int imageTexIndex;

}

