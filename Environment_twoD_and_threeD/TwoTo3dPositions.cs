using UnityEngine;
using System.Collections;

public class TwoTo3dPositions : MonoBehaviour
{
    //move the 3d position based on 2d position velocity
    public static SystemActionCall<TwoTo3dPositions> twoTo3dPostionOnActionStart = new SystemActionCall<TwoTo3dPositions>();
    [SerializeField]
    public PhysicsGameObject2d physicsGameObject2D;
    public Vector3 initialPosition { get; set; }
    public Vector3 initialRot {get; set;}
    public Vector3 threeToTwoDMovement { get; set; }
    private Rigidbody rb;

    public void FixedUpdate()
    {

        transform.position = initialPosition + threeToTwoDMovement;// movementManager.SetThreeDMovementOnTwoDMovement(physicsGameObject2D.currentPos, physicsGameObject2D.initialPos);
        transform.localEulerAngles = new Vector3(initialRot.x + physicsGameObject2D.rb2D.transform.localEulerAngles.z, 270, 90);
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
