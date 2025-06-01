
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Cinemachine;
using Codice.CM.Common.Merge;
using Unity.Mathematics;
using UnityEngine;

public class InterviewFace : MonoBehaviour
{
    [SerializeField]
    private float distance = 3;

    [SerializeField]
    private float speed = 0.1f;
    [SerializeField]
    private float functionTime = 5;
    private float time = 0;

    private int noiseAmount;

    IEnumerator[] coroutineAxis = new IEnumerator[3];
    public void ChooseCoords()
    {

        noiseAmount = Mathf.RoundToInt(UnityEngine.Random.value * 2);

        for (int i = 0; i <= noiseAmount; i++)
        {
          

            IEnumerator ie;
            if (coroutineAxis[i] != null)
            {
                StopCoroutine(coroutineAxis[i]);
            }
            StartCoroutine(ie = RotateFace(i, distance));
            coroutineAxis[i] = ie;

        }
    }
    public float Rotation(float a, float b, float time)
    {
       return Mathf.LerpAngle(a, b, time);
    }
    public IEnumerator RotateFace(int currentAxisIndex, float m)
    {
        float time = 0.0f;

    
        float rotA = UnityEngine.Random.Range(4, m);
        float rotB = UnityEngine.Random.Range(4, m);

        rotA *= Mathf.Min(0.5f,Mathf.PerlinNoise(Time.time, 0) * 5);
        rotB *= Mathf.Min(0.5f,Mathf.PerlinNoise(Time.time, 0) * 5);

        rotB = 360 - rotB;

        float moveToRot = rotA;
        float currentRot = transform.localEulerAngles[currentAxisIndex];

        //if (transform.localEulerAngles[currentAxisIndex] - 360 > 0)
        // if(transform.localEulerAngles[currentAxisIndex] > 0.5f)
        if(UnityEngine.Random.value > 0.5)
        {
            moveToRot = rotB;
        }
       

  

        while (time < 1)
        {

            time += Time.deltaTime * speed;

            Vector3 temp = transform.localEulerAngles;

            temp[currentAxisIndex] = Rotation(currentRot, moveToRot, time);
            transform.localEulerAngles = temp;

            yield return new WaitForFixedUpdate();
        }
        if (currentAxisIndex == noiseAmount)
        {
            ChooseCoords();
        }
        
        yield return null;
    }
    public void Start()
    {

        ChooseCoords();

    }
}