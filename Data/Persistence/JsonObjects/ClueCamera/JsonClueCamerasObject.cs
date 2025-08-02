using UnityEngine;

namespace Persistence
{
    [System.Serializable]
    public class JsonClueCamerasObject : JsonObject
    {

        public SceneNames scenename;

        public JsonClueCameraObject[] clueCameraObject;

        public JsonClueCamerasObject(SceneNames scenename, JsonClueCameraObject[] clueCameraObject)
        {
            this.scenename = scenename;
            this.clueCameraObject = clueCameraObject;
        }
    }
}