using UnityEngine;
using System.Collections;
using Data;
public interface IPlayerMovementInterface
{
	public void setPlayerDataPlayerMovement(ref Rigidbody player3dRb,
		ref Rigidbody2D player2dRb, ref Animator playerAnim, ref float speed);

	public Vector3 setThreeDMovementOnTwoDMovement(Vector2 twodpos, Vector2 twodstartpos);
	

}

