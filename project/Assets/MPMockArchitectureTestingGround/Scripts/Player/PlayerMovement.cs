using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player localPlayer;
    private Rigidbody2D rb;
    void Start()
    {
        localPlayer = GameObject.Find("GameSceneManager").GetComponent<IPlayerManager>().localPlayer;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (gameObject.GetComponent<Player>() == GameObject.Find("GameSceneManager").GetComponent<IPlayerManager>().localPlayer)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rb.AddForce(new Vector2(-2, 0), ForceMode2D.Impulse);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                rb.AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
            }
        }
    }
}
