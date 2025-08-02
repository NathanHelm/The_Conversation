using UnityEngine;
[System.Serializable]
public class ClueCameraObject
{
    public Vector3 camPos;
    public Vector3 camEulerRot;
    public Texture clueCameraTexture; //this DOES NOT need to be saved. 

    public int clueid;
    public int sceneId;

    public ClueCameraObject(Vector3 camPos, Vector3 camEulerRot, int sceneID , Texture clueCameraTexture)
    {
        this.sceneId = sceneID;
        this.camPos = camPos;
        this.camEulerRot = camEulerRot;
        this.clueCameraTexture = clueCameraTexture;
    }
}