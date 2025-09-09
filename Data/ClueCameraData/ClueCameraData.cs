using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Data
{
    public class ClueCameraData : StaticInstanceData<ClueCameraData>, IExecution
    {
        [SerializeField]
        public List<GameObject> spawnedCamera = new();
        

        [SerializeField]
        private ClueCameraScriptableObject clueCameraScriptableObject;

        public ClueCameraSpawner clueCameraSpawner { get; set; }

        public GameObject clueCameraPrefab { get; set; }
        
        public SobelMachine sobelMachine { get; set; }

        public override void m_OnEnable()
        {
            clueCameraSpawner ??= FindObjectOfType<ClueCameraSpawner>();

            var sobelMachines = FindObjectsOfType<SobelMachine>();

            foreach (SobelMachine single in sobelMachines)
            {
                if (single.sobelMat.name == "SOBELPAPER")
                {
                    sobelMachine = single;
                }
            }

            if (sobelMachine == null)
            {
                Debug.LogError("sobel paper material nowhere to be found: check name and shader script");
            }
            
            

            var RTwidth = clueCameraScriptableObject.width;
            var RTheight = clueCameraScriptableObject.height;
            var clueCameraPrefab = clueCameraScriptableObject.clueCameraPrefab;

            ClueCameraManager.INSTANCE.onStartClueCameraManager.AddAction(ccm =>
            {
                ccm.RTheight = RTheight;
                ccm.RTwidth = RTheight;
                clueCameraSpawner.clueCameraPrefab = clueCameraPrefab; ccm.clueCameraSpawner = clueCameraSpawner;
            });

            base.m_OnEnable();
        }
    }
}