using UnityEngine;

namespace Persistence
{
    [System.Serializable]
    public class JsonClueCamerasObject : JsonObject
    {
        public JsonClueCameraObject[] clueCameraObject;
        public JsonClueCamerasObject(JsonClueCameraObject[] clueCameraObject)
        {
            this.clueCameraObject = clueCameraObject;
        }
    }
}