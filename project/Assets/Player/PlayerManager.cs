using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerManager : MonoBehaviour
{
    // player rigidbody
    private Rigidbody2D bird;

    private int playerScore;

    void Start()
    {
        bird = GetComponent<Rigidbody2D>();
        playerScore = 0;
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public void Kill()
    {
        // to do
        Debug.Log("dies");
        SceneManager.LoadScene("SingleplayerGameScene");
    }

    public void WakeUp()
    {
        if (bird.IsSleeping())
        {
            bird.WakeUp();
        }
    }

    public void IncrementScore()
    {
        playerScore++;
    }
}
