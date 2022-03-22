using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float flightForce;

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
            bird.AddForce(new Vector2(0, flightForce * bird.mass), ForceMode2D.Impulse);
        }
    }
}
