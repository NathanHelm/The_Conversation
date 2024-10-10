using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private DialogueScriptableObject dialogueScriptableObject;
    [SerializeField]
    private float moveSpeed = 15;
    private Vector2 movement;
    private Vector3 movement3d;
    private bool isWalking = true;
    [SerializeField]
    private float velocityThreshold = 0;
    Rigidbody2D playerRigidBody;
    Rigidbody playerRigidBody3d;
    Animator animator;
    public IEnumerator coroutine { get; set; }
    PlayerData playerData;


    private void Start()
    {
        SetPlayer(playerData);
    }
    // Update is called once per frame
    public void SetPlayer(PlayerData p)
    {
        playerData = PlayerData.INSTANCE;
        playerRigidBody = playerData.rb2D;
        playerRigidBody3d = playerData.rb3D;
        animator = playerData.playerAnimator;

        this.moveSpeed = playerData.currentPlayerSO.speed;
        playerRigidBody3d.drag = playerRigidBody.drag = 1;
        playerRigidBody3d.mass = playerRigidBody.mass = 2;
    }

    public void setPlayerPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void PlayerMovementFunction()
    {
        //'if is walking' is true, then perform code.
        if (isWalking)
        {
            Debug.Log(playerRigidBody.velocity.magnitude);
            playerRigidBody.velocity = movement * moveSpeed;
            // playerRigidBody3d.AddForce(movement3d * moveSpeed, ForceMode.Force);
            playerRigidBody3d.velocity = movement3d * moveSpeed;
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            movement3d.z = Input.GetAxisRaw("Vertical");
            movement3d.x = Input.GetAxisRaw("Horizontal");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                animator.SetFloat("Last Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Last Vertical", Input.GetAxisRaw("Vertical"));
            }
            if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) 
            {

            }
          //  if (playerRigidBody.velocity.magnitude <= 0) //removes instantious low velocity edge case.
            {
         //       return;
            }

            if (playerRigidBody.velocity.magnitude < velocityThreshold && playerRigidBody.velocity.magnitude < velocityThreshold)
            {

                //todo add data here
                if (coroutine == null)
                {
                    StartCoroutine(coroutine = TransitionCoroutine());
                }
             
            }
            else
            {
                StopAllCoroutines();
                coroutine = null;
            }



        }

    }
    public IEnumerator TransitionCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        GameEventManager.INSTANCE.OnEvent(typeof(TransitionTo3d));
        GameEventManager.INSTANCE.OnEvent(typeof(PlayerLook3dState));



        yield return null;
    }



}
