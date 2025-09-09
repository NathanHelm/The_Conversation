using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ImageSO", menuName = "ScriptableObjects/ImageSO", order = 13)]

public class UIScriptableObject : ScriptableObject
{
    public IconObject[] icons;
    public float leftIconShift;
    public GameObject iconPrefab;
    public Texture ledgerImageUI;
    public Material ledgerImageUIMaterial;

    [Header("UI dialogue object")]
    public DialogueSelectionObject dialogueSelectionObject;
}
