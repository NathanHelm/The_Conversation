using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data;
public class PlayerRaycast : MonoBehaviour
{
    RaycastHit raycastHit;
    LayerMask layerMask;
    float distance = 100;
    private void OnEnable()
    {
        //Input.getmouse
        
        layerMask = LayerMask.GetMask("cluecollider");
    }
    public void OmitRaycast()
    {
        //character does on click down....

        Vector2 mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (Physics.Raycast(transform.position, mousePos.normalized, out raycastHit, distance, layerMask))
        {
            var isHit = raycastHit.collider.GetComponents<ClueMono>();
            if(isHit.Length > 1)
            {
                throw new Exception("ray has hit more than 1 cluemono");
            }
            if(isHit[0] != null) //check to see if the raycast omitted has hit a cluemono.
            {
                //if hit, set dialogueData
                ClueMono clueMonoInRay = isHit[0];
                DialogueData.INSTANCE.currentCharacterID = clueMonoInRay.bodyID;
                DialogueData.INSTANCE.currentQuestionID = 0; //clue mono has default 0 as vet is not asking a question.

            }
            Debug.Log("hit!!!!");
        }

    }
   

}
