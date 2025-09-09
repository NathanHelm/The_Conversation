using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using Cinemachine;
public class TransitionManager : StaticInstance<TransitionManager>, IExecution{

    public static SystemActionCall<TransitionManager> actionOnStartConversation = new SystemActionCall<TransitionManager>();
    public Camera cam {get; set;}
    public CinemachineBrain cinemachineBrain { get; set; }

    public Vector3 direction;

    public override void m_OnEnable()
    {
        MManager.INSTANCE.onStartManagersAction.AddAction(
            (MManager e) => {e.transitionManager = this;}
        );
        base.m_OnEnable();
    }
    public override void m_Start()
    {
       actionOnStartConversation.RunAction(this);
    }
    public void ToDimension(DimensionScriptableObject dimensionScriptableObject)
	{
		cam.orthographic = dimensionScriptableObject.isOrthographic;
        // cam.orthographicSize = dimensionScriptableObject.cameraSize;
        
        Data.DimensionData.INSTANCE.cinemachineVirtualCamera2D.m_Lens.OrthographicSize = dimensionScriptableObject.cameraSize;
		dimensionScriptableObject.cinemachineVirtualCamera.Priority = 1;
	}
    public void SetCamToZero(DimensionScriptableObject dimensionScriptableObject)
    {
      dimensionScriptableObject.cinemachineVirtualCamera.Priority = 0;
    }

}