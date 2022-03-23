using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    // Force applied to rigidbody on player inout
    private float flightForce;

    [SerializeField]
    // Speed limit for upwards movement
    private float maxSpeed;

    [SerializeField]
    // Spped limit for downwards movement
    private float minSpeed;

    // player rigidbody
    private Rigidbody2D bird;

    void Start()
    {
        bird = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && bird.IsAwake())
        {
            // applies upwards force on the rigidbody
            Fly();
        }

        // Enforces the speed limits on the rigidbody
        LimitSpeed(bird.velocity);
    }

    private void Fly()
    {
        bird.AddForce(new Vector2(0, flightForce * bird.mass), ForceMode2D.Impulse);
    }

    private void LimitSpeed(Vector2 velocity)
    {
        bird.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, minSpeed, maxSpeed));
    }
}
