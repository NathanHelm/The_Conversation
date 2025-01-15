using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class PlayerMovement : MonoBehaviour
{
    public static SystemActionCall<PlayerMovement> playerMovementActionCallOnStart = new SystemActionCall<PlayerMovement>();
    public static SystemActionCall<PlayerMovement> playerMovementActionCallOnUpdate = new SystemActionCall<PlayerMovement>();

    [SerializeField]
    public float moveSpeed { get; set; } = 15;
    private Vector2 movement;
    private bool isWalking = true;
    [SerializeField]
    private float velocityThreshold = 0;


    public Rigidbody2D playerRigidBody2d;
    public Rigidbody playerRigidBody3d;
    public Animator playerMovementAnim;
    public Vector3 twoDThreeDDifference = Vector3.zero;



    public IEnumerator coroutine { get; set; }
    public Vector2 startPosition = Vector3.zero;
    public Vector3 start3dPosition = Vector3.zero;


    private void Start()
    {

        playerMovementActionCallOnStart.RunAction(this);
        startPosition = playerRigidBody2d.position;
        start3dPosition = playerRigidBody3d.position;


    }



    // Update is called once per frame
    public void SetRigidbodyAttributes()
    {

        playerRigidBody3d.drag = playerRigidBody2d.drag = 1;
        playerRigidBody3d.mass = playerRigidBody2d.mass = 2;
    }

    public void PlayerMovementFunction()
    {
        //'if is walking' is true, then perform code.
        playerMovementActionCallOnUpdate.RunAction(this);
        if (isWalking)
        {

            playerRigidBody2d.velocity = movement * moveSpeed;

            if (playerMovementActionCallOnUpdate == null)
            {
                Debug.LogError("on call update is null.");

                return;
            }
            playerRigidBody3d.transform.position = start3dPosition + get2dAnd3dOffset(); //setThreeDMovementOnTwoDMovement(playerRigidBody2d.position, startPosition);


            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");


            playerMovementAnim.SetFloat("Horizontal", movement.x);
            playerMovementAnim.SetFloat("Vertical", movement.y);
            playerMovementAnim.SetFloat("Speed", movement.sqrMagnitude);

            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                playerMovementAnim.SetFloat("Last Horizontal", Input.GetAxisRaw("Horizontal"));
                playerMovementAnim.SetFloat("Last Vertical", Input.GetAxisRaw("Vertical"));
            }

            if (playerRigidBody2d.velocity.magnitude < velocityThreshold && playerRigidBody2d.velocity.magnitude < velocityThreshold)
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
    public void stopMovement()
    {
        StopAllCoroutines();
        coroutine = null;
    }

    public Vector3 get2dAnd3dOffset()
    {
        return twoDThreeDDifference;
    }
    public void set2dAnd3dOffset(Vector3 twoDThreeDDifference)
    {
        this.twoDThreeDDifference = twoDThreeDDifference;
    }


    public IEnumerator TransitionCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        GameEventManager.INSTANCE.OnEvent(typeof(TransitionTo3d));
        GameEventManager.INSTANCE.OnEvent(typeof(PlayerLook3dState));



        yield return null;
    }



}
