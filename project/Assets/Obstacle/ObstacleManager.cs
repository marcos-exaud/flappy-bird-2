using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField]
    // speed at which the obstacle moves
    private float speed;

    // obstacle rigidbody
    private Rigidbody2D obstacle;

    void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D intruderCollider)
    {
        GameObject intruder = intruderCollider.gameObject;

        // act depending on the other collider:
        if (intruder.layer == LayerMask.NameToLayer("Reset Checkpoint")) // loop the obstacle to the other side of the game
        {
            Cycle();
        }
        else if (intruder.layer == LayerMask.NameToLayer("Reposition Checkpoint")) // change the obstacle's height
        {
            RepositionCheckpoint repositionCheckpoint = intruder.GetComponent<RepositionCheckpoint>();

            float newHeight = Tools.LimitedRandomVariance(repositionCheckpoint.GetLastObstacleHeight(),
                                                            Consts.MIN_GAP_HEIGHT,
                                                            Consts.MAX_GAP_HEIGHT,
                                                            Consts.MAX_ABS_VARIANCE);
                                                            
            RepositionY(newHeight);

            repositionCheckpoint.SetLastObstacleHeight(newHeight);
        }
        else if (intruder.layer == LayerMask.NameToLayer("Player")) // increment the score
        {
            EventManager.OnObstacleClear?.Invoke(intruder);
        }
    }

    public void WakeUp()
    {
        if (obstacle.IsSleeping())
        {
            obstacle.WakeUp();
            obstacle.velocity = new Vector2(-speed, 0);
        }
    }

    // cycles the obstacle to the other side of game, effectively respawning it
    public void Cycle()
    {
        obstacle.position = new Vector2(14f, obstacle.position.y);
    }

    // repositions the obstacle in the y axis
    public void RepositionY(float value)
    {
        obstacle.position = new Vector2(obstacle.position.x, value);
    }
}
