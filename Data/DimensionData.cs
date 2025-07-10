using UnityEngine;
using System.Collections;
using Cinemachine;

namespace Data
{
	public class DimensionData : StaticInstanceData<DimensionData>, IExecution
	{
		[SerializeField]
		public DimensionScriptableObject TransitionTo2dSo;
		[SerializeField]
		public DimensionScriptableObject TransitionTo3dSo;
		public Camera cam;
		public UIData uIData;
		public UIManager uIManager;


		public override void m_Awake()
		{
			try
			{
				cam = FindObjectOfType<Camera>().GetComponent<Camera>();

				CinemachineVirtualCamera cinemachineVirtualCamera2D = GameObject.FindWithTag("vcam2D").GetComponent<CinemachineVirtualCamera>();
				CinemachineVirtualCamera cinemachineVirtualCamera3D = GameObject.FindWithTag("vcam3D").GetComponent<CinemachineVirtualCamera>();
				TransitionTo2dSo.cinemachineVirtualCamera = cinemachineVirtualCamera2D;
				TransitionTo3dSo.cinemachineVirtualCamera = cinemachineVirtualCamera3D;
			}
			catch (System.NullReferenceException e)
			{
				Debug.Log(e);
			}
			finally
			{
				base.m_Awake();
			}
		}
		public override void m_OnEnable()
		{

			TransitionManager.actionOnStartConversation.AddAction((TransitionManager e) => { e.cam = this.cam; });
			base.m_OnEnable();
		}
		private void Start()
		{
			uIData = UIData.INSTANCE;
		}
	   public void ChangeTexture(ref Renderer renderer, Texture texture)
        {
             if(renderer.material.HasTexture("_MainTex"))
             {
                renderer.material.SetTexture("_MainTex", texture);
             }
             else
             {
                Debug.Log("_MainTex does not exist for this shader");
             }
        }

    
}
}

