using UnityEngine;
using System.Collections;
using System;
public class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour, IExecution
{
	public static T INSTANCE { get; set; }

	public SystemActionCall<T> actionCall = new SystemActionCall<T>();

	public virtual void m_Awake()
	{
		INSTANCE = GetComponent<T>();
	}
	public virtual void OnDisable()
	{
		
	}
	public virtual void m_Start()
	{
		
		actionCall.RunAction(INSTANCE);
	}
	public virtual void m_GameExecute()
	{
		
	}

	public virtual void m_OnEnable()
	{
		//adds static values to the event hashmap.
		// MManager.INSTANCE.onStartManagersAction.AddAction((MManager m) => { m_Start(); /*add m_start action*/ });
		// GameEventManager.INSTANCE.AddEventFunc(typeof(T).ToString().ToLower(),ReturnStaticType);

	}
	public T ReturnStaticType()
	{
		return INSTANCE;
	}
}

