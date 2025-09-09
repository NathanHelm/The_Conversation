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
		public Cinemachine.CinemachineBrain cinemachineBrain;
		public CinemachineVirtualCamera cinemachineVirtualCamera2D { get; set; }
		public CinemachineVirtualCamera cinemachineVirtualCamera3D { get; set; }
		public UIData uIData;
		public UIManager uIManager;


		public override void m_Awake()
		{
			try
			{
				cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
				cinemachineBrain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cinemachine.CinemachineBrain>();
				cinemachineVirtualCamera2D = GameObject.FindWithTag("vcam2D").GetComponent<CinemachineVirtualCamera>();
				cinemachineVirtualCamera3D = GameObject.FindWithTag("vcam3D").GetComponent<CinemachineVirtualCamera>();
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

			TransitionManager.actionOnStartConversation.AddAction((TransitionManager e) => { e.cam = this.cam; e.cinemachineBrain = cinemachineBrain; });
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

