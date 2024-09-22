using UnityEngine;
using System.Collections;

public class StaticInstance<T> : MonoBehaviour
{
	public static T INSTANCE { get; set; }
	
	public virtual void Awake()
	{
		if(INSTANCE == null)
		{
			INSTANCE = GetComponent<T>();
		}
		
	}
	public virtual void m_Start()
	{

	}

    public virtual void OnEnable()
    {
		//adds static values to the event hashmap.
		GameEventManager.INSTANCE.AddEventFunc(typeof(T).ToString().ToLower(),ReturnStaticType);
    }
	public T ReturnStaticType()
	{
		return INSTANCE;
	}
}

