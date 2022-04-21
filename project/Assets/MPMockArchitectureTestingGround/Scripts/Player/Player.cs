using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayerAction(Player player);
    public static event PlayerAction onPlayerStart;
    public static event PlayerAction onPlayerDestroy;
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
}
