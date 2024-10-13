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
    [SerializeField]
    private Vector2 previous2dPosition = Vector2.zero;
    private Vector2 capturedVec;
    private bool isCollision = false;


    private void Start()
    {
        SetPlayer(playerData);
    }

    private void Can3dPlayerMove(Rigidbody2D rb, Vector2 movement2d)
    {
        
        Debug.Log("VELOCITY-->" + playerRigidBody.velocity);
        Debug.Log("magnitude" + playerRigidBody.velocity.magnitude + "x");
        
       

        movement3d.x = movement2d.x;
        movement3d.z = movement2d.y;

        float distanceX = Vector2.Distance(new Vector2(rb.position.x, 0), new Vector2(previous2dPosition.x, 0));
        float distanceY = Vector2.Distance(new Vector2(0, rb.position.y), new Vector2(0,previous2dPosition.y));

        Debug.Log("dist x--> " + distanceX + "dist y -->" + distanceY);
        if (distanceX < 0.045)
        {
            movement3d.x = 0;
        }
        if(distanceY < 0.045)
        {
            movement3d.z = 0;
        }

        previous2dPosition = rb.position;


 
    }

    private void Check3DMove()
    {
        
        float capturedangle = Mathf.Atan2(capturedVec.y, capturedVec.x);
        float currentAngle = Mathf.Atan2(playerRigidBody.velocity.x, playerRigidBody.velocity.y);
        float captureAngleMinus45 = capturedangle - 45;
        float captureAnglePlus45 = capturedangle + 45;

        Debug.Log("captued angle" + capturedangle * Mathf.Rad2Deg + "current angle" + currentAngle * Mathf.Rad2Deg);
        Debug.Log("VELOCITY-->" + playerRigidBody.velocity);
        if (currentAngle == capturedangle || currentAngle == captureAngleMinus45 || currentAngle == captureAnglePlus45)
        {
            movement3d = Vector3.zero;
        }
        else
        {
            movement3d.x = 0;
            movement3d.z = 0;
        }
      
        //Debug.Log("captued angle" + capturedangle * Mathf.Rad2Deg + "current angle" + currentAngle * Mathf.Rad2Deg);
        Debug.Log("VELOCITY-->" + playerRigidBody.velocity);
  
         
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
            playerRigidBody.velocity = movement * moveSpeed;
            playerRigidBody3d.velocity = movement3d * moveSpeed;

            Can3dPlayerMove(playerRigidBody, movement);




            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");


            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                animator.SetFloat("Last Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Last Vertical", Input.GetAxisRaw("Vertical"));
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
