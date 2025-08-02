using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data;
using ObserverAction;
public class PlayerRaycast : MonoBehaviour
{
    RaycastHit raycastHit;

    public SubjectActionData<PlayerActions, ClueMono> subject = new();
    LayerMask layerMask;
    float distance = 100;
    public ClueMono[] hitClues { get; set; }

    [SerializeField]
    Texture temp;
    private void OnEnable()
    {
        
        
        layerMask = LayerMask.GetMask(new string[] { "cluecollider" });
        //OmitRaycast(new Vector3(10, 10, 10));
    }
    
    public void OmitRaycast(Vector3 target)
    {
        //character does on click down....

       
        // If target is a normalized direction vector:
        Debug.DrawRay(transform.position, target * distance, Color.yellow, 50f);

        if (Physics.Raycast(transform.position, target.normalized, out raycastHit, distance, layerMask))
        {
            hitClues = raycastHit.collider.GetComponents<ClueMono>();
            if(hitClues.Length > 1)
            {
                Debug.Log("ray cast hit multiple objects, getting first one");
            }
            if (hitClues[0] != null) //check to see if the raycast omitted has hit a cluemono.
            {
                //if hit, set dialogueData
                ClueMono clueMonoInRay = hitClues[0];
                //I argue we should add question id and character id instead.
                subject.NotifyObservers(PlayerActions.onOmitRay, clueMonoInRay);

            }
            //TODO Change this
            
            //add dialogue that says drawing here is useless... 



            Debug.Log("hit!!!!");
        }
        /*
        ClueMono c = FindObjectOfType<ClueMono>().GetComponent<ClueMono>();
        Texture IMAGECREATORTEXTURE = temp;
        LedgerImageManager.INSTANCE.AddRayInfoToLedgerImage(c.imageDescription, c.questionID, c.clueQuestionID , IMAGECREATORTEXTURE,c.ledgerOverlays, ClueMono.clueBodyID); //adding 'hit data information to ledger manager'
        */

    }
   

}
