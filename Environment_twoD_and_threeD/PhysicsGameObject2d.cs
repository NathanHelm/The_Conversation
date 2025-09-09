using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGameObject2d : MonoBehaviour, IExecution
{

    public Rigidbody2D rb2D { get; set; }
    public Vector2 initialPos { get; set; }
    public Vector2 currentPos { get; set; }
    public Vector3 initialEuler { get; set; }
    public Vector3 currentEuler { get; set; }


    public void SetInitialPos()
    {

        rb2D = GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            initialPos = rb2D.position;
            initialEuler = transform.localEulerAngles;
        }
    }
    public void SetInitialLocalPos()
    {
        initialPos = transform.localPosition;
        initialEuler = transform.localEulerAngles;
    }
    private void FixedUpdate()
    {
        currentEuler = transform.localEulerAngles;
        if (rb2D != null)
        {
            currentPos = rb2D.position;
        }
        else
        {
            currentPos = transform.position;
        }
        
    }

    public void m_Awake()
    {
//        throw new System.NotImplementedException();
    }

    public void m_OnEnable()
    {
         SetInitialPos();
    }

    public void m_GameExecute()
    {
     //   throw new System.NotImplementedException();
    }
}
