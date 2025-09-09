using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float amp;
    public void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(0, 0, speed);
    }
}
