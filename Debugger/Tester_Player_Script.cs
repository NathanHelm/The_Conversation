using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester_Player_Script : MonoBehaviour
{

    public float mouseSensitivity = 900;
    private Vector2 rot;
    readonly float cameraVerticalRot = 88;

    [SerializeField] 
    [Header("Add camera trans below")]
    Transform trans;
    [SerializeField]
    private PlayerRaycast playerRaycast;


    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    private void FixedUpdate()
    {
       LookContorl();
        
    }
    private void Update()
    {
       // trans
       playerRaycast.OmitRaycast(trans.rotation * Vector3.forward * 100);
       Debug.DrawRay(trans.position, trans.rotation * Vector3.forward * 100, Color.blue);
    }


    public void LookContorl()
    {
        rot.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        rot.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rot.y = Mathf.Clamp(rot.y, -cameraVerticalRot, cameraVerticalRot);
        var xQuat = Quaternion.AngleAxis(rot.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rot.y, Vector3.left);
        trans.localRotation = xQuat * yQuat;

        
     }
    

}
