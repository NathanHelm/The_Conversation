using UnityEngine;
using System.Collections;
using ObserverAction;
using Data;
using System;

public class ClueCameraSpawner : MonoBehaviour//, IObserverData<ObserverAction.PlayerActions, ClueMono>
{

    public GameObject clueCameraPrefab { get; set; }


    public GameObject SpawnClueCamera(Vector3 postion, Vector3 eulerRotation)
    {
        GameObject spawnedCam = Instantiate(clueCameraPrefab, GameObject.FindGameObjectWithTag("ClueCameraParent").transform);
        spawnedCam.transform.position = postion;
        spawnedCam.transform.eulerAngles = eulerRotation;

        Data.ClueCameraData.INSTANCE.spawnedCamera.Add(spawnedCam);

        //how to get unique id? 
        //alternate key
        //id (as the SIZE of the dictionary) + sceneID
        Debug.Log("spawned camera!");
        return spawnedCam;
    }
  

   
   

}