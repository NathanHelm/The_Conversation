using UnityEngine;
[CreateAssetMenu(fileName = "ClueCameraSO", menuName = "ScriptableObjects/cluecameraSO", order = 14)]

public class ClueCameraScriptableObject : ScriptableObject
{
    public int width;
    public int height;

    public GameObject clueCameraPrefab;

}