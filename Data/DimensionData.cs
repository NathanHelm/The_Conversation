using UnityEngine;
using System.Collections;
using Cinemachine;

namespace Data
{
	public class DimensionData : StaticInstanceData<DimensionData>
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
		public override void OnEnable()
		{

			TransitionManager.actionOnStartConversation.AddAction((TransitionManager e) => { e.cam = this.cam; });
			base.OnEnable();
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

