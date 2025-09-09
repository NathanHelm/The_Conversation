using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data;
using ObserverAction;
public class PlayerRaycast : MonoBehaviour
{
    RaycastHit raycastHit;

    public SubjectActionData<PlayerActions, ClueMono> subjectClue = new();
    public SubjectActionData<PlayerActions, CharacterMono> subjectCharacter = new();
    LayerMask layerMask;
    float distance = 100;
    public ClueMono[] hitClues { get; set; }
    public Character3DMono[] hitCharacters { get; set; }

    [SerializeField]
    Texture temp;
    private void OnEnable()
    {
        
        
        layerMask = LayerMask.GetMask(new string[] { "cluecollider", "charactercollider" });
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
            hitCharacters = raycastHit.collider.GetComponents<Character3DMono>();

            if (hitClues.Length > 1 || hitCharacters.Length > 0 || hitCharacters.Length > 0 && hitClues.Length > 0)
            {
                Debug.LogWarning("ray cast hit multiple objects, getting first one");
            }
            if (hitClues.Length > 0) //check to see if the raycast omitted has hit a cluemono.
            {
                //if hit, set dialogueData
                ClueMono clueMonoInRay = hitClues[0];
                //I argue we should add question id and character id instead.
                //#1 clue data is added:
                subjectClue.NotifyObservers(PlayerActions.onOmitRayClue, clueMonoInRay);
                GameEventManager.INSTANCE.OnEvent(typeof(PlayCutsceneState));
                //#2 state runs:
                GameEventManager.INSTANCE.OnEvent(typeof(WriteToPageLedgerState));
            }
            else if (hitCharacters.Length > 0)
            {
                CharacterMono characterMonoInRay = hitCharacters[0].GetCharacterMono2d();
                subjectCharacter.NotifyObservers(PlayerActions.onOmitRayCharacter, characterMonoInRay);
                GameEventManager.INSTANCE.OnEvent(typeof(MemorySceneState));
            }
            //TODO Change this

                //add dialogue that says drawing here is useless... 



                Debug.Log("hit!!!!");
        }
        else
        {
            GameEventManager.INSTANCE.OnEvent(typeof(Playerlook3dNoCursorWrap));
        }
      
        /*
            ClueMono c = FindObjectOfType<ClueMono>().GetComponent<ClueMono>();
            Texture IMAGECREATORTEXTURE = temp;
            LedgerImageManager.INSTANCE.AddRayInfoToLedgerImage(c.imageDescription, c.questionID, c.clueQuestionID , IMAGECREATORTEXTURE,c.ledgerOverlays, ClueMono.clueBodyID); //adding 'hit data information to ledger manager'
            */

        }
   

}
