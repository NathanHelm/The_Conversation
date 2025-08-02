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
        public int clueid;
        public int sceneId;

        public JsonClueCameraObject(Vector3 camPos, Vector3 camEulerRot, int sceneID, string textureName)
        {
            this.sceneId = sceneID;
            this.camPos = camPos;
            this.camEulerRot = camEulerRot;
            this.textureName = textureName;
        }
        
        
    }
}