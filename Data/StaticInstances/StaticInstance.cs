using UnityEngine;
using System.Collections;
using System;
public class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T INSTANCE { get; set; }
	public SystemActionCall<T> actionCall = new SystemActionCall<T>();

	public virtual void Awake()
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

	public virtual void OnEnable()
	{
		Debug.Log("data hello from on enable!");
        //adds static values to the event hashmap.
		// MManager.onStartManagersAction.AddAction((MManager m) => { m_Start(); /*add m_start action*/ });
		// GameEventManager.INSTANCE.AddEventFunc(typeof(T).ToString().ToLower(),ReturnStaticType);

	}
	public T ReturnStaticType()
	{
		return INSTANCE;
	}
}

