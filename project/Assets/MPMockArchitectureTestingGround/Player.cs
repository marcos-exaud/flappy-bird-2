using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool ready = false;

    #region MonoBehaviour Methods
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Getters and Setters
    public bool GetReady()
    {
        return ready;
    }
    #endregion
}
