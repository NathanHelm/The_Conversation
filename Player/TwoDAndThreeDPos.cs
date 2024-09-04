using UnityEngine;
using System.Collections;

public class TwoDAndThreeDPos : MonoBehaviour
{
    public float scaleFactor;


    public Vector3 GetScaleFactor(Vector3 vector2)
    {
        vector2 *= scaleFactor;
        return Vector3.zero;
    }

}

