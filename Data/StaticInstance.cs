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
    public void OnEnable()
    {
		GameEventManager.INSTANCE.AddEventFunc(typeof(T).ToString().ToLower(),ReturnStaticType);
    }
	public T ReturnStaticType()
	{
		return INSTANCE;
	}
}

