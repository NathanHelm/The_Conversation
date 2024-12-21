using UnityEngine;
using System.Collections;

public class MovementManager : StaticInstance<MovementManager>
{ 
    public Vector3 SetThreeDMovementOnTwoDMovement(Vector2 twodpos, Vector2 twodstartpos)
    {
        Vector3 positionTransformation;
        Vector2 difference = (twodstartpos - twodpos) * -1;
        positionTransformation = new Vector3(difference.x, 0, difference.y);
        return positionTransformation;
    }
    public override void OnEnable()
    {
        PlayerMovement.playerMovementActionCallOnUpdate.AddAction((PlayerMovement p)=> { p.set2dAnd3dOffset( SetThreeDMovementOnTwoDMovement(p.playerRigidBody2d.position, p.startPosition)); });
        TwoTo3dPositions.twoTo3dPostionOnActionStart.AddAction((TwoTo3dPositions t) => { t.threeToTwoDMovement = SetThreeDMovementOnTwoDMovement(t.physicsGameObject2D.currentPos, t.initialPosition); });
    }
    
   
}

