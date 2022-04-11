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

    // Wrappers
    protected InputWrapper inputWrapper;

    void Start()
    {
        bird = GetComponent<Rigidbody2D>();

        InitWrappers();
    }

    protected virtual void Update()
    {
        if (inputWrapper.GetKeyDown(KeyCode.UpArrow))
        {
            // applies upwards force on the rigidbody
            Fly();
        }

        // Enforces the speed limits on the rigidbody
        LimitSpeed(bird.velocity);
    }

    protected virtual void InitWrappers()
    {
        if (inputWrapper == null) inputWrapper = new InputWrapper();
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
