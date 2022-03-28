using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField]
    // Force applied to rigidbody on player inout
    private float flightForce = 5;

    [SerializeField]
    // Speed limit for upwards movement
    private float maxSpeed = 7;

    [SerializeField]
    // Spped limit for downwards movement
    private float minSpeed = -7;

    // player rigidbody
    private Rigidbody2D bird;

    void Start()
    {
        bird = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // applies upwards force on the rigidbody
            Fly();
        }

        // Enforces the speed limits on the rigidbody
        LimitSpeed(bird.velocity);
    }

    protected void Fly()
    {
        bird.AddForce(new Vector2(0, flightForce * bird.mass), ForceMode2D.Impulse);
    }

    protected void LimitSpeed(Vector2 velocity)
    {
        bird.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, minSpeed, maxSpeed));
    }
}
