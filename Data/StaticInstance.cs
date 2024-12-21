using UnityEngine;
using System.Collections;
using System;
public class StaticInstance<T> : MonoBehaviour
{
	public static T INSTANCE { get; set; }
	public SystemActionCall<T> actionCall = new SystemActionCall<T>();

	public virtual void Awake()
	{
		if(INSTANCE == null)
		{
			INSTANCE = GetComponent<T>();
		}
		//every "static instance" in scene subscribes the MManager class as one centeral class for "manager" start execution. 
	
		
	}
	public virtual void m_Start()
	{
		
	}
	
    public virtual void OnEnable()
    {
        //adds static values to the event hashmap.
       // MManager.onStartManagersAction.AddAction((MManager m) => { m_Start(); /*add m_start action*/ });
        GameEventManager.INSTANCE.AddEventFunc(typeof(T).ToString().ToLower(),ReturnStaticType);
		
    }
	public T ReturnStaticType()
	{
		return INSTANCE;
	}
}

