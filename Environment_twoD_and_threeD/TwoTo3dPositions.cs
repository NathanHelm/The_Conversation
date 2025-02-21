using UnityEngine;
using System.Collections;

public class TwoTo3dPositions : MonoBehaviour
{
    //move the 3d position based on 2d position position
    public static SystemActionCall<TwoTo3dPositions> twoTo3dPostionOnActionStart = new SystemActionCall<TwoTo3dPositions>();
    [SerializeField]
    public PhysicsGameObject2d physicsGameObject2D;
    public Vector3 initialPosition { get; set; }
    public Vector3 initialRot {get; set;}
    public Vector3 threeToTwoDMovement { get; set; }
    private Rigidbody rb;

    public void FixedUpdate()
    {
        Vector3 currentPosition = physicsGameObject2D.currentPos - physicsGameObject2D.initialPos;
       // transform.localEulerAngles = Vector3.zero;
        threeToTwoDMovement = new Vector3(currentPosition.x + initialPosition.x, initialPosition.y, currentPosition.y + initialPosition.z);
        transform.position =  threeToTwoDMovement;// movementManager.SetThreeDMovementOnTwoDMovement(physicsGameObject2D.currentPos, physicsGameObject2D.initialPos);
        transform.localEulerAngles = new Vector3(initialRot.x + physicsGameObject2D.rb2D.transform.localEulerAngles.z, initialRot.y, initialRot.z);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

       // rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        rb.mass = 15;
        rb.angularDrag = 5;

        physicsGameObject2D.rb2D.mass = 15;
        physicsGameObject2D.rb2D.angularDrag = 5;

        initialPosition = transform.position;
        initialRot = transform.localEulerAngles;

    }


}
