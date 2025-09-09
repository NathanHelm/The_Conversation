using UnityEngine;

namespace Persistence
{
    [System.Serializable]
    public class JsonClueCameraObject : JsonObject
    {
        /*
        EXACTLY the same as clue camera object EXCEPT we use texture path instead of texture!
        */

        public Vector3 camPos;
        public Vector3 camEulerRot;
        public string textureName; //this is fact DOES NOT need to be saved. 
        public int clueCameraPrimaryKey;
        public int sceneId;
        public int clueId;

        public JsonClueCameraObject(Vector3 camPos, Vector3 camEulerRot, int sceneID, string textureName, int clueCameraPrimaryKey, int clueId)
        {
            this.sceneId = sceneID;
            this.camPos = camPos;
            this.camEulerRot = camEulerRot;
            this.textureName = textureName;
            this.clueCameraPrimaryKey = clueCameraPrimaryKey;
            this.clueId = clueId;
        }
        
        
        
    }
}