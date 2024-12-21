using UnityEngine;
using System.Collections;

namespace Data
{
    public class PlayerData : StaticInstance<PlayerData> 
    {

        public PlayerScriptableObject playerSO, slowPlayerSO, currentPlayerSO;
        public PlayerMovement playerMovement;
        public PlayerLook playerLook;
        public Animator playerAnimator;
        public Rigidbody2D rb2D;
        public Rigidbody rb3D;
        public Transform trans2d, trans3d;
  


        public override void Awake()
        {

            currentPlayerSO = playerSO;
            SetUpPlayerData();
            base.Awake();
        }
        public override void OnEnable()
        {
            //assigning variables to playermovement onstart.

            PlayerMovement.playerMovementActionCallOnStart.AddPriorityAction((PlayerMovement p) =>
            {
                p.playerRigidBody3d = rb3D;
                p.playerRigidBody2d = rb2D;
                p.playerMovementAnim = playerAnimator;
                p.moveSpeed = playerSO.speed;

            });

        }

        public void SetUpPlayerData()
        {
            playerAnimator = FindObjectOfType<PlayerMovement>().GetComponent<Animator>();
            rb2D = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
            playerMovement = FindObjectOfType<PlayerMovement>();
            playerLook = FindObjectOfType<PlayerLook>();
            rb3D = FindObjectOfType<PlayerLook>().GetComponent<Rigidbody>();
            trans2d = FindObjectOfType<PlayerMovement>().transform;
            trans3d = FindObjectOfType<PlayerLook>().transform;
          
        }
 
        
    }
}

