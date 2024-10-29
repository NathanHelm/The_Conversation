using UnityEngine;
using System.Collections;

public class TwoTo3dPositions : MonoBehaviour
{
    //move the 3d position based on 2d position velocity
    [SerializeField]
    private PhysicsGameObject2d physicsGameObject2D;
    private Vector3 initialPosition, initialRot;
    private MovementManager movementManager;
    private Rigidbody rb;

    public void FixedUpdate()
    {
       // Setup();
        transform.position = initialPosition + movementManager.SetThreeDMovementOnTwoDMovement(physicsGameObject2D.currentPos, physicsGameObject2D.initialPos);
        transform.localEulerAngles = new Vector3(initialRot.x + physicsGameObject2D.rb2D.transform.localEulerAngles.z, 270, 90);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

       // rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        rb.mass = 20;
        rb.angularDrag = 10;

        physicsGameObject2D.rb2D.mass = 20;
        physicsGameObject2D.rb2D.angularDrag = 10;

        initialPosition = transform.position;
        initialRot = transform.localEulerAngles;

        movementManager = GameEventManager.INSTANCE.OnEventFunc<MovementManager>("movementmanager");
    }


}
