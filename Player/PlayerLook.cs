using UnityEngine;
using System.Collections;
using Data;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity { get; set; }
    PlayerData playerData;
    Vector2 rot;
    private float vlast = 0;
    float cameraVerticalRot = 88;


    private void Start()
    {
        playerData = PlayerData.INSTANCE;
        mouseSensitivity = playerData.currentPlayerSO.cameraSensitivity;
    }

    public void LookContorl()
    {

        rot.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        rot.y += Input.GetAxis("Mouse Y") * mouseSensitivity;

        rot.y = Mathf.Clamp(rot.y, -cameraVerticalRot, cameraVerticalRot);
        // cameraVerticalRot = Mathf.Clamp(cameraVerticalRot, -90f, 90f);
        var xQuat = Quaternion.AngleAxis(rot.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rot.y, Vector3.left);

        playerData.trans3d.localRotation = xQuat * yQuat;
       
        if(Input.GetKey(KeyCode.A))
        {
            Switch();
        }
        if(Input.GetKey(KeyCode.W))
        {
            Switch();
        }
        if(Input.GetKey(KeyCode.S))
        {
            Switch();
        }
        if(Input.GetKey(KeyCode.D))
        {
            Switch();
        }
     }
    private void Switch()
    {
        GameEventManager.INSTANCE.OnEvent(typeof(TransitionTo2d));
        GameEventManager.INSTANCE.OnEvent(typeof(PlayerMove2dState));
        
        /*
        GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo2d), () => { StateManager.INSTANCE.playerDataState.SwitchState(StateManager.INSTANCE.playerMove2DState); });
        GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo2d), ()=> );
        GameEventManager.INSTANCE.AddEvent(typeof(PlayerLook));
        */
       StateManager.INSTANCE.dimensionState.SwitchState(StateManager.INSTANCE.transitionTo2D);
    }
}

