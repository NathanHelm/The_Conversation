using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ImageSO", menuName = "ScriptableObjects/ImageSO", order = 13)]

public class ImageUIScriptableObject : ScriptableObject
{
    public Texture interviewUIIcon;
    public Material interviewUIMaterial;
    public Texture ledgerImageUI;
    public Material ledgerImageUIMaterial;
}
