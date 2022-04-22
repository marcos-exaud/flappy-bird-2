using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Delegates
    public delegate void PlayerInstanceAction(Player player);
    public delegate void PlayerPropertyAction(Player player, string property);

    // Events
    public static event PlayerInstanceAction onPlayerStart;
    public static event PlayerInstanceAction onPlayerDestroy;
    public static event PlayerInstanceAction onPlayerPropertyUpdate;

    // Properties
    private bool ready = false;

    #region MonoBehaviour Methods
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        onPlayerStart?.Invoke(this);
    }

    void Update()
    {
        if (this == GameObject.Find("GameSceneManager").GetComponent<IPlayerManager>().localPlayer)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ready = true;
            }
        }
    }

    void OnDestroy()
    {
        onPlayerDestroy?.Invoke(this);
    }
    #endregion

    #region Getters and Setters
    public bool GetReady()
    {
        return ready;
    }
    #endregion

    public void ChangeSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.2f, 0.2f);
    }

    public void TogglePhysics()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic ^= true;
    }
}
