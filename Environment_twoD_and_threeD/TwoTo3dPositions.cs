using UnityEngine;
using System.Collections;
using Unity.Mathematics;
public enum RotationType
{
    x,
    y,
    z,
}
public class TwoTo3dPositions : MonoBehaviour,IExecution
{
    //moves an object's 3d position based on a 2d position
    public static SystemActionCall<TwoTo3dPositions> twoTo3dPostionOnActionStart = new SystemActionCall<TwoTo3dPositions>();
    [SerializeField]
    public PhysicsGameObject2d physicsGameObject2D;
    public Vector3 initialPosition { get; set; }
    public Vector3 initialRot { get; set; }
    public Vector3 threeToTwoDMovement { get; set; }
    [SerializeField]
    private RotationType rotationType;
    private Rigidbody rb;

    public void FixedUpdate()
    {
        Vector3 currentPosition = physicsGameObject2D.currentPos - physicsGameObject2D.initialPos;
        Vector3 currentEulerPosition =  physicsGameObject2D.currentEuler - physicsGameObject2D.initialEuler;
        threeToTwoDMovement = new Vector3(currentPosition.x + initialPosition.x, initialPosition.y, currentPosition.y + initialPosition.z);
        transform.position = threeToTwoDMovement;// movementManager.SetThreeDMovementOnTwoDMovement(physicsGameObject2D.currentPos, physicsGameObject2D.initialPos);
        if (rotationType == RotationType.x)
        {
            transform.localEulerAngles = new Vector3(initialRot.x + currentEulerPosition.z, initialRot.y, initialRot.z);
        }
        if (rotationType == RotationType.y)
        {
            transform.localEulerAngles = new Vector3(initialRot.x, initialRot.y - currentEulerPosition.z, initialRot.z);
        }
         if (rotationType == RotationType.z)
        {
            transform.localEulerAngles = new Vector3(initialRot.x, initialRot.y, initialRot.z + currentEulerPosition.z);
        }
    }
    
    public void m_GameExecute()
    {

        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

        // rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;

        rb.mass = 15;
        rb.angularDamping = 5;
        if (physicsGameObject2D.rb2D != null)
        {
            physicsGameObject2D.rb2D.mass = 15;
            physicsGameObject2D.rb2D.angularDamping = 5;
        }

        initialPosition = transform.position;
        initialRot = transform.localEulerAngles;

    }
    public void SetPosition(Vector3 initialPosition)
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.mass = 15;
        rb.angularDamping = 5;
        if (physicsGameObject2D.rb2D != null)
        {
            physicsGameObject2D.rb2D.mass = 15;
            physicsGameObject2D.rb2D.angularDamping = 5;
        }
 
        this.initialPosition = initialPosition;
        
        initialRot = transform.localEulerAngles;
    }

    public void m_Awake()
    {
       // throw new System.NotImplementedException();
    }

    public void m_OnEnable()
    {
       // throw new System.NotImplementedException();
    }

}
