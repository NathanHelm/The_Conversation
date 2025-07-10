using UnityEngine;

public class StaticInstanceManager<T> : StaticInstance<T> where T : MonoBehaviour, IExecution
{
    public override void m_Awake()
    {
        base.m_Awake();
    }
    public override void m_OnEnable()
    {
        base.m_OnEnable();
    }
}