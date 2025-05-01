using UnityEngine;
[CreateAssetMenu(fileName = "PageSO", menuName = "ScriptableObjects/PageSO", order = 11)]
public class PageScriptableObject : ScriptableObject
{
    public Texture[] noiseTextures;
    public Material drawShaderMaterial;
}
