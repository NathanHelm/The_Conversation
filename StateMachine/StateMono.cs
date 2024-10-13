using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StateMono<T> : MonoBehaviour
{
    protected T Value;
    public State<T> currentState = new State<T>();

    public void FixedUpdate()
    {

        if (currentState != null)
        {
            currentState.OnUpdate(Value) ;
        }
    }
    public void SwitchState(State<T> nextState)
    {

        if (currentState != null)
        {
            currentState.OnExit(Value);
        }

        

        currentState = nextState;

        currentState.Value = Value;

        currentState.OnEnter(Value);



    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState != null)
        {
            currentState.OnTriggerEnterEnemy(Value, collision);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (currentState != null)
        {

            currentState.OnTriggerExitEnemy(Value, collision);

        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (currentState != null)
        {

            currentState.OnTriggerStayEnemy(Value, collision);

        }
    }






}

