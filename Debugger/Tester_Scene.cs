using UnityEngine;

public class Tester_Scene : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            GameEventManager.INSTANCE.OnEvent(typeof(VetHouseSceneState));
        }

    }
}
