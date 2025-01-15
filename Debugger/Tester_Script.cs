using UnityEngine;
using System.Collections;

public class Tester_Script : MonoBehaviour
{
    [SerializeField]
    GameObject g;
    PlayerRaycast temp;
    private void Start()
    {
        temp = g.GetComponent<PlayerRaycast>();
    }
    private void Update()
    {
       
            temp.OmitRaycast(new Vector3(0, 10, 0));
  
    }
}

