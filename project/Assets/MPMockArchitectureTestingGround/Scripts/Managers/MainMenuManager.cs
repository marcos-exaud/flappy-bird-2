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

    void OnEnable()
    {
        mpAPI.onJoinRoomFailed += CreateRoom;
        mpAPI.onJoinedRoom += LoadGameScene;
    }

    void OnDisable()
    {
        mpAPI.onJoinRoomFailed -= CreateRoom;
        mpAPI.onJoinedRoom -= LoadGameScene;
    }

    void OnDestroy()
    {
        mpAPI.onJoinRoomFailed -= CreateRoom;
        mpAPI.onJoinedRoom -= LoadGameScene;
    }
    #endregion

    public void Connect()
    {
        if (!mpAPI.IsConnected()) { mpAPI.Connect(); }
    }

    public void JoinRoom()
    {
        if (mpAPI.IsConnected() && !mpAPI.IsInRoom())
        {
            mpAPI.JoinRoom();
        }
    }

    private void CreateRoom()
    {
        if (mpAPI.IsConnected() && !mpAPI.IsInRoom())
        {
            mpAPI.CreateRoom();
        }
    }

    private void LoadGameScene()
    {
        if (mpAPI.IsConnected() && mpAPI.IsInRoom() && mpAPI.PlayerCount() == 1)
        {
            mpAPI.LoadScene(1);
        }
    }
}
