using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPun
{
    // player rigidbody
    protected Rigidbody2D bird;

    private int playerScore;

    protected virtual void Awake()
    {
        PlayerList.players.Add(gameObject);
    }
    protected virtual void Start()
    {
        bird = GetComponent<Rigidbody2D>();
        playerScore = 0;
    }

    void OnDestroy()
    {
        PlayerList.players.Remove(gameObject);
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    /// <summary>
    /// Method <c>Kill</c> Kills the player, invoking OnPlayerDeath event
    /// </summary>
    public void Kill()
    {
        // sleeps the rigidbody of the player to disable physics simulation
        bird.Sleep();

        // disables player movement so the player can't continue playing after losing
        gameObject.GetComponent<PlayerMovement>().enabled = false;

        EventManager.OnPlayerDeath?.Invoke();
    }

    /// <summary>
    /// Method <c>WakeUp</c> Wakes up the rigidbody of the player to allow physics simulation
    /// </summary>
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

    public bool PlayerIsAlive()
    {
        return bird.IsAwake();
    }
}
