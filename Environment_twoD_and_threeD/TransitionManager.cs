using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
public class TransitionManager : StaticInstance<TransitionManager>{

    public static SystemActionCall<TransitionManager> actionOnStartConversation = new SystemActionCall<TransitionManager>();
    public Camera cam {get; set;}

    public Vector3 direction;

    public override void OnEnable()
    {
        MManager.onStartManagersAction.AddAction(
            (MManager e) => {e.transitionManager = this;}
        );
        base.OnEnable();
    }
    public override void m_Start()
    {
       actionOnStartConversation.RunAction(this);
    }
    public void ToDimension(DimensionScriptableObject dimensionScriptableObject)
	{
		cam.orthographic = dimensionScriptableObject.isOrthographic;
		dimensionScriptableObject.cinemachineVirtualCamera.Priority = 1;
	}
    public void SetCamToZero(DimensionScriptableObject dimensionScriptableObject)
    {
      dimensionScriptableObject.cinemachineVirtualCamera.Priority = 0;
    }
 
}