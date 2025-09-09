using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "backgroundVideo", menuName = "ScriptableObjects/backgroundVideo", order = 20)]
public class BackgroundVideoScriptableObject : ScriptableObject
{
    public BackgroundVideosObject[] backgroundVideosObjects;
    public Material[] backgroundFacesMaterial;
    public Material posterizationMaterial;

    public GameObject videoPlayerPrefab;
    
}
