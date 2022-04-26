using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Delegates
    public delegate void PlayerInstanceAction(Player player);
    public delegate void LocalPlayerInstanceAction(Player player);

    // Events
    public static event PlayerInstanceAction onPlayerStart;
    public static event PlayerInstanceAction onPlayerDestroy;
    public static event LocalPlayerInstanceAction onLocalPlayerReadyUp;

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

        if (this == GameObject.Find("GameSceneManager").GetComponent<IPlayerManager>().localPlayer)
        {
            StartCoroutine(WaitForReadyUp());
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

    public void SetReady(bool value)
    {
        ready = value;
    }
    #endregion

    private IEnumerator WaitForReadyUp()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.DownArrow));

        onLocalPlayerReadyUp?.Invoke(this);
        ready = true;
    }

    public void ChangeSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.2f, 0.2f);
    }

    public void TogglePhysics()
    {
        gameObject.GetComponent<Rigidbody2D>().isKinematic ^= true;
    }
}
