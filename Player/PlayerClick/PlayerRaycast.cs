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
    public ClueMono[] hitClues { get; set; }
    private void OnEnable()
    {
        
        
        layerMask = LayerMask.GetMask(new string[] { "cluecollider" });
        //OmitRaycast(new Vector3(10, 10, 10));
    }
    
    public void OmitRaycast(Vector3 target)
    {
        //character does on click down....

       
        // If target is a normalized direction vector:
        Debug.DrawRay(transform.position, target * distance, Color.yellow);

        if (Physics.Raycast(transform.position, target.normalized, out raycastHit, distance, layerMask))
        {
            hitClues = raycastHit.collider.GetComponents<ClueMono>();
            if(hitClues.Length > 1)
            {
                Debug.Log("ray cast hit multiple objects, getting first one");
            }
            if(hitClues[0] != null) //check to see if the raycast omitted has hit a cluemono.
            {
                //if hit, set dialogueData
                ClueMono clueMonoInRay = hitClues[0];
                DialogueData.INSTANCE.currentCharacterID = clueMonoInRay.bodyID;
                DialogueData.INSTANCE.currentQuestionID = 0; //clue mono has default 0 as vet is not asking a question.

                //I argue we should add question id and character id instead. 

                LedgerManager.INSTANCE.AddRayInfoToLedgerImage(clueMonoInRay.bodyID, clueMonoInRay.imgDescription, (clueMonoInRay.bodyID, 0), clueMonoInRay.clueQuestions); //adding 'hit data information to ledger manager'

            }
            
            Debug.Log("hit!!!!");
        }

    }
   

}
