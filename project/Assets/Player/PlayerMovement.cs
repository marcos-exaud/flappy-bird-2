using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float flightForce;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private float minSpeed;

    private Rigidbody2D bird;

    void Start()
    {
        bird = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (bird.IsSleeping())
            {
                bird.WakeUp();
            }
            Fly();
        }

        LimitSpeed(bird.velocity);
    }

    void Fly()
    {
        bird.AddForce(new Vector2(0, flightForce * bird.mass), ForceMode2D.Impulse);
    }

    void LimitSpeed(Vector2 velocity)
    {
        bird.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, minSpeed, maxSpeed));
    }
}
