using UnityEngine;
using System.Collections;
using Data;

public class MovementManager : StaticInstance<MovementManager>, IExecution
{
    public Vector3 SetThreeDMovementOnTwoDMovement(Vector2 twodpos, Vector2 twodstartpos)
    {
        Vector3 positionTransformation;
        Vector2 difference = (twodstartpos - twodpos) * -1;
        positionTransformation = new Vector3(difference.x, 0, difference.y);
        return positionTransformation;
    }


    public override void m_OnEnable()
    {
        PlayerMovement.playerMovementActionCallOnUpdate.AddAction((PlayerMovement p) => { p.set2dAnd3dOffset(SetThreeDMovementOnTwoDMovement(PlayerData.INSTANCE.rb2D.position, PlayerData.INSTANCE.start2dPos)); });
        TwoTo3dPositions.twoTo3dPostionOnActionStart.AddAction((TwoTo3dPositions t) => { t.threeToTwoDMovement = SetThreeDMovementOnTwoDMovement(t.physicsGameObject2D.currentPos, t.initialPosition); });
    }
    
    
    
   
}

