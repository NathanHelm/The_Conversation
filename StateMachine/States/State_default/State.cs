using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class State<T> 
{
    public T Value { get; set; }
    public virtual void OnExit(T data)
    {
       
    }
    public virtual void OnEnter(T data)
    {

    }
    public virtual void OnTriggerEnterEnemy(T data, Collider2D other)
    {

    }
    public virtual void OnTriggerExitEnemy(T data, Collider2D other)
    {

    }
    
    public virtual void OnUpdate(T data)
    {

    }
 
    public virtual void OnTriggerStayEnemy(T data, Collider2D other)
    {

    }
    public virtual void OnFixedUpdate(T data)
    {

    }

}
