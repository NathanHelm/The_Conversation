using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsGameObject2d : MonoBehaviour {

    public Rigidbody2D rb2D { get; set; }
    public Vector2 initialPos { get; set; }
    public Vector2 currentPos { get; set; }

    private void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();
        initialPos = rb2D.position;
    }
    private void FixedUpdate()
    {
        currentPos = rb2D.position;
    }











}
