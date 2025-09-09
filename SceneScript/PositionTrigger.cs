using System;
using Data;
using UnityEngine;

[System.Serializable]
public enum SpawnDirections
{
    Up, Down, Left, Right
}
public class PositionTrigger : MonoBehaviour
{
    [SerializeField]
    private PositionTrigger nextPositionTrigger;
    [SerializeField]
    private float directionAmount = .8f;
    [SerializeField]
    SpawnDirections spawnDirection = SpawnDirections.Down;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger == false)
        {
            Debug.Log("hello");
            Vector2 nextPos = nextPositionTrigger.transform.position;
            Vector2 direction = DirectionsToVector2(spawnDirection);
            float isPositive = direction.x + direction.y;
            Vector2 halfLocalScale = nextPositionTrigger.transform.localScale * 0.5f;
            if (direction.x > 0 || direction.x < 0)
            {
                PlayerData.INSTANCE.playerMovement.transform.position = new Vector2(nextPos.x + halfLocalScale.x * /*Mathf.Max(0,*/ directionAmount * isPositive, nextPos.y);
            }
            else //its direction y
            {
                float min = halfLocalScale.y;
              //  float val = min * isPositive * directionAmount;
                PlayerData.INSTANCE.playerMovement.transform.position = new Vector2(nextPos.x,nextPos.y + (min * isPositive + directionAmount * isPositive));
            }
        }
    }
    public Vector2 DirectionsToVector2(SpawnDirections spawnD)
    {
        if (spawnD == SpawnDirections.Up)
        {
            return Vector2.up;
        }
        else if (spawnD == SpawnDirections.Down)
        {
            return Vector2.down;
        }
        else if (spawnD == SpawnDirections.Left)
        {
            return Vector2.left;
        }
        return Vector2.right;
    }

}