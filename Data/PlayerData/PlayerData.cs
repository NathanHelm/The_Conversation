using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Data
{
    public class PlayerData : StaticInstanceData<PlayerData> , IExecution
    {

        public PlayerScriptableObject playerSO, slowPlayerSO, currentPlayerSO;
        public PlayerMovement playerMovement;
        public PlayerLook playerLook;
        public PlayerRaycast playerRaycast { get; set; }
        public Animator playerAnimator;
        public Rigidbody2D rb2D;
        public Rigidbody rb3D;
        public Transform trans2d, trans3d;

        public Vector2 start2dPos;
        public Vector3 start3dPos;

        public int currentQuestionID; //question that the player asks


        public override void m_Awake()
        {

            currentPlayerSO = playerSO;
            SetUpPlayerData();
            base.m_Awake();
        }
        public override void m_OnEnable()
        {
            //assigning variables to playermovement onstart.
            PlayerMovement.playerMovementActionCallOnStart.AddPriorityAction((PlayerMovement p) =>
            {
                p.playerRigidBody3d = rb3D;
                p.playerRigidBody2d = rb2D;
                p.playerMovementAnim = playerAnimator;
                p.moveSpeed = playerSO.moveSpeed;
                p.velocityThreshold = playerSO.velocityThreshold;
                p.playerLook = this.playerLook;
                p.startPosition = start2dPos;
                p.start3dPosition = start3dPos;

            });

            

        }

        public void SetUpPlayerData()
        {
            playerAnimator = FindObjectOfType<PlayerMovement>().GetComponent<Animator>();
            rb2D = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
            playerMovement = FindObjectOfType<PlayerMovement>();
            playerLook = FindObjectOfType<PlayerLook>().GetComponent<PlayerLook>();
            playerRaycast = FindObjectOfType<PlayerRaycast>();

            rb3D = FindObjectOfType<PlayerLook>().GetComponent<Rigidbody>();
            trans2d = FindObjectOfType<PlayerMovement>().transform;
            trans3d = FindObjectOfType<PlayerLook>().transform;

            start2dPos = rb2D.position;
            start3dPos = rb3D.position;

          
        }
 
        
    }
}

