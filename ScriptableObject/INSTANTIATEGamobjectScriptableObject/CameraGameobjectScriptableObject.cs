using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace InstGameObject
{
    [CreateAssetMenu(fileName = "CameraGameobjectSO", menuName = "ScriptableObjects/InstGameObject/CameraGameObjectSO", order = 1)]
    public class CameraGameObjectScriptableObject : ScriptableObject
    {
        [Header("Objects that will be added to LedgerCams")]
        public GameObject ledgerCam_NoNoise;
        public GameObject ledgerCamNoise;

    }
}