using UnityEngine;
using System.Collections;

public class MovementManager : StaticInstance<MovementManager>
{

    //gets the movement of the 2d position.
    //to properly use this function: startPosition + TwoDMovement 
    public Vector3 SetThreeDMovementOnTwoDMovement(Vector2 twodpos, Vector2 twodstartpos)
    {
        Vector3 positionTransformation = Vector2.zero;
        Vector2 difference = (twodstartpos - twodpos) * -1;
        positionTransformation = new Vector3(difference.x, 0, difference.y);
        return positionTransformation;

    }
 


    // playerRigidBody3d.transform.position = start3dPosition + PositionTransform(playerRigidBody.position, startPosition);




}

