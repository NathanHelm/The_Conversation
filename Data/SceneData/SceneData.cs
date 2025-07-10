
using UnityEngine;

namespace Data{
    public class SceneData : StaticInstanceData<SceneData>, IExecution
    {

        public SceneNames nextScene { get; set; } = 0;
        public SceneNames currentScene { get; set; } = SceneNames.VetHouseScene;

        public static SceneNames previousSceneState = SceneNames.VetHouseScene; //TODO change this!


}

}
