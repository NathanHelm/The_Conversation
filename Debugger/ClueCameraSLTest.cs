using UnityEngine;

public class ClueCameraSLTest : MonoBehaviour
{
   private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SavePersistenceManager.INSTANCE.SaveInterfaceData(ClueCameraManager.INSTANCE);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
          
        }
        
    }
}