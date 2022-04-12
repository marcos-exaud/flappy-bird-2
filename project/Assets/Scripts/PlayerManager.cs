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

    protected int playerScore;

    // Wrappers
    protected GameObjectWrapper gameObjectWrapper;
    protected Rigidbody2DWrapper birdWrapper;

    protected virtual void Awake()
    {
        InitWrappersAwake();

        PlayerList.players.Add(gameObjectWrapper);
    }
    protected virtual void Start()
    {
        bird = gameObjectWrapper.GetComponent<Rigidbody2D>();
        playerScore = 0;

        InitWrappersStart();
    }

    protected virtual void OnDestroy()
    {
        PlayerList.players.Remove(gameObjectWrapper);
    }

    protected void InitWrappersAwake()
    {
        gameObjectWrapper = new GameObjectWrapper(gameObject);
    }

    protected void InitWrappersStart()
    {
        birdWrapper = new Rigidbody2DWrapper(bird);
    }

    public virtual int GetPlayerScore()
    {
        return playerScore;
    }

    /// <summary>
    /// Method <c>Kill</c> Kills the player, invoking OnPlayerDeath event
    /// </summary>
    public virtual void Kill()
    {
        // sleeps the rigidbody of the player to disable physics simulation
        birdWrapper.Sleep();

        // disables player movement so the player can't continue playing after losing
        gameObjectWrapper.GetComponent<PlayerMovement>().enabled = false;

        EventManager.OnPlayerDeath?.Invoke();
    }

    /// <summary>
    /// Method <c>WakeUp</c> Wakes up the rigidbody of the player to allow physics simulation
    /// </summary>
    public void WakeUp()
    {
        if (birdWrapper.IsSleeping())
        {
            birdWrapper.WakeUp();
        }
    }

    public virtual void IncrementScore()
    {
        playerScore++;
    }

    public virtual bool PlayerIsAlive()
    {
        if (birdWrapper == null)
        {
            return false;
        }
        return birdWrapper.IsAwake();
    }
}
