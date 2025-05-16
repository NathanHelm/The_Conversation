
using UnityEngine;

namespace Data{
public class SceneData : StaticInstance<SceneData>{

    public SceneNames nextScene {get; set;} = 0;
    public SceneNames currentScene {get; set;}= SceneNames.VetHouseScene;


}

}
