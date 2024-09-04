using UnityEngine;
using System.Collections;

public class Tester_Script : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            StateManager.INSTANCE.playerDataState.SwitchState(StateManager.INSTANCE.playerLook3DState);
            StateManager.INSTANCE.dimensionState.SwitchState(StateManager.INSTANCE.transitionTo3D);
        }
    }
}

