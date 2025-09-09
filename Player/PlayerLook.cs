using UnityEngine;
//using UnityEngine.InputSystem;
using System.Collections;
//using UnityEngine.InputSystem.UI;
using Data;
using System;
using Codice.Client.Common.GameUI;

public class PlayerLook : MonoBehaviour, IExecution
{
    public float mouseSensitivity { get; set; }
    PlayerData playerData;
    Vector2 rot;

    float openingRotAngle; //the opening rotation the character faces. runs in exit of player movement. 
    private float vlast = 0;
    float cameraVerticalRot = 88;
    
    private Vector2 centerPosition = new Vector2();


    public void m_GameExecute()
    {
        playerData = PlayerData.INSTANCE;
        mouseSensitivity = playerData.currentPlayerSO.cameraSensitivity;
        
        centerPosition = new Vector2(Screen.width, Screen.height);
        centerPosition /= 2; 
    }
    public void PlayerFacesDirection(Vector2 twoDDirection)
    {
                                                                  //offset by 90 and convert to angle.
       float angle = (Mathf.Atan2(twoDDirection.y, twoDDirection.x) - Mathf.PI / 2) * Mathf.Rad2Deg; 
       
       openingRotAngle = angle;
       rot = Vector2.zero; //resets 3d offset via mouse to 0,0
    }

    public void SetOpeningPlayerRotation()
    {
       

        //setting mouse to center of the screen to rotation takes effect.
       UnityEngine.InputSystem.Mouse.current.WarpCursorPosition(centerPosition);
       //playerData.trans3d.localEulerAngles = openingRot;
    
    }
    public void Look()
    {
        if(rot == null || playerData == null)
        {
            //Debug.LogError("rot is null");
            return;
        }
        if(playerData.trans3d == null)
        {
            return;
        }
        rot.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        rot.y += Input.GetAxis("Mouse Y") * mouseSensitivity;

        rot.y = Mathf.Clamp(rot.y, -cameraVerticalRot, cameraVerticalRot);
       
        // cameraVerticalRot = Mathf.Clamp(cameraVerticalRot, -90f, 90f);
      
        var xQuat = Quaternion.AngleAxis(rot.x - openingRotAngle, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rot.y, Vector3.left);

        playerData.trans3d.localRotation = xQuat * yQuat;
    }

    public void LookContorl()
    {
        Look();

        if (InputBuffer.INSTANCE.IsPressCharacter(KeyCode.Mouse0))
        {
            SwitchToPlayerRaycast();
        }

        if (Input.GetKey(KeyCode.A))
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
        Debug.Log("switching to 2d");
        GameEventManager.INSTANCE.OnEvent(typeof(TransitionTo2d));
        GameEventManager.INSTANCE.OnEvent(typeof(PlayerMove2dState));
        
        /*
        GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo2d), () => { StateManager.INSTANCE.playerDataState.SwitchState(StateManager.INSTANCE.playerMove2DState); });
        GameEventManager.INSTANCE.AddEvent(typeof(TransitionTo2d), ()=> );
        GameEventManager.INSTANCE.AddEvent(typeof(PlayerLook));
        */
      // StateManager.INSTANCE.dimensionState.SwitchState(StateManager.INSTANCE.transitionTo2D);
    }
    private void SwitchToPlayerRaycast()
    {
        StateManager.INSTANCE.playerDataState.SwitchState(StateManager.INSTANCE.playerClickOnClueState);
    }

    public void m_Awake()
    {
       
    }

    public void m_OnEnable()
    {
        
    }


}

