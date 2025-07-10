using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class RotatePlain : StaticInstance<RotatePlain>, IExecution
{
    float yRot = 0;
    public IEnumerator RotatePlainCoroutine()
    {
        while (true)
        {
            yRot = FollowPlayerAngle(PlayerData.INSTANCE.rb3D.transform.position, transform.position)
            * Mathf.Rad2Deg;


            Transform target = PlayerData.INSTANCE.trans3d;
            transform.LookAt(target);

            /*
            UnityEngine.Quaternion angleAxis = UnityEngine.Quaternion.AngleAxis(yRot, UnityEngine.Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, angleAxis, Time.deltaTime * 50);
            */
            yield return new FixedUpdate();
        }
    }
    public float FollowPlayerAngle(Vector3 playerPosition, Vector3 myPos)
    {
        Vector3 direction = playerPosition - myPos;
        return Mathf.Atan2(direction.y, direction.x);
    }
  
    public override void m_GameExecute()
    {
         StartCoroutine(RotatePlainCoroutine());
        base.m_GameExecute();
    }
}
