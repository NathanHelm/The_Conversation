
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Data{
    public class SceneData : StaticInstanceData<SceneData>, IExecution
    {
        //making them captialized to easily recognize they are static (should really do this everywhere.) 
        public static State<SceneData> CURRENTSCENESTATE = new();
        public static SceneNames CURRENTSCENE { get; set; } = SceneNames.None; 

       
        public SceneNames nextScene { get; set; } = 0;

        public override void m_OnEnable()
        {
            //this determines the very scene when the scene starts.
            //it really should be scene name menu/none :/ 
            CURRENTSCENESTATE = StateManager.INSTANCE.vetHouseSceneState;

        }
    }

}
