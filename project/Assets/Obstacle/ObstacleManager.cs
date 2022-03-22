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

        // if the other collider is an obsticle, cycle its position to the other side of the game screen
        if (intruder.layer == LayerMask.NameToLayer("Reset Checkpoint"))
        {
            Cycle();
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
