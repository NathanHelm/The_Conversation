using UnityEngine;

public class Tester_Scene : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameEventManager.INSTANCE.OnEvent(typeof(SampleSceneState));
        }
         if(Input.GetKeyDown(KeyCode.O))
        {
            GameEventManager.INSTANCE.OnEvent(typeof(InterviewSceneState));
        }

    }
}
