using UnityEngine;
[System.Serializable]
public class ClueCameraObject
{
    public Vector3 camPos;
    public Vector3 camEulerRot;
    public Texture clueCameraTexture; //this DOES NOT need to be saved. 
    

    public int clueCameraPrimaryKey;
    public int clueId;
    public int sceneId;

    public ClueCameraObject(Vector3 camPos, Vector3 camEulerRot, int sceneID, int clueCameraPrimaryKey, int clueId, Texture clueCameraTexture)
    {
        this.sceneId = sceneID;
        this.camPos = camPos;
        this.camEulerRot = camEulerRot;
        this.clueCameraTexture = clueCameraTexture;
        this.clueCameraPrimaryKey = clueCameraPrimaryKey;
        this.clueId = clueId;
    }
}