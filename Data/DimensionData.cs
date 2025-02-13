using UnityEngine;
using System.Collections;
using Cinemachine;

namespace Data
{ 
public class DimensionData : StaticInstance<DimensionData>
{
	[SerializeField]
	public DimensionScriptableObject TransitionTo2dSo;
	[SerializeField]
	public DimensionScriptableObject TransitionTo3dSo;
	public Camera cam;
	public UIData uIData;
	public UIManager uIManager;
	

    public override void Awake()
    {
			try
			{
				cam = FindObjectOfType<Camera>().GetComponent<Camera>();

				CinemachineVirtualCamera[] cinemachineVirtualCamera = FindObjectsOfType<CinemachineVirtualCamera>();
				TransitionTo2dSo.cinemachineVirtualCamera = cinemachineVirtualCamera[0];
				TransitionTo3dSo.cinemachineVirtualCamera = cinemachineVirtualCamera[1];
			}
			catch (System.NullReferenceException e)
			{
				Debug.Log(e);
			}
			finally
			{
				base.Awake();
			}
    }
    private void Start()
    {
        uIData = GameEventManager.INSTANCE.OnEventFunc<UIData>("data.uidata");
    }
    public void ToDimension(DimensionScriptableObject dimensionScriptableObject)
	{
		cam.orthographic = dimensionScriptableObject.isOrthographic;
		dimensionScriptableObject.cinemachineVirtualCamera.Priority = 1;
		
	}
	 
}
}

