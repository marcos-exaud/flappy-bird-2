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

    public void WakeUp()
    {
        if (obstacle.IsSleeping())
        {
            obstacle.WakeUp();
            obstacle.velocity = new Vector2(-speed, 0);
        }
    }
}
