using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // APIs
    private MultiplayerAPI mpAPI;

    #region MonoBehaviour Methods
    void Awake()
    {
        mpAPI = gameObject.GetComponent<MultiplayerAPI>();
    }

    void Start()
    {
        mpAPI.Start();
    }
    #endregion

    public void Connect()
    {
        mpAPI.Connect();
    }

    public void JoinRoom()
    {
        mpAPI.JoinRoom();
    }
}
