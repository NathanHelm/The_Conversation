using System;
using UnityEngine;

namespace ActionControl
{
    public class ActionOnSwitchScene
    {
        public Action<SceneManager> switchSceneDefault = lm =>
        {
            //switch scene to default scene.
            //run default animation
        };
        public Action<SceneManager> switchSceneOpeningCutscene = lm =>
        {
            
        };
       
    }
}