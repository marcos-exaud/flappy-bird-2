using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class PUNMultiplayerAPI : MonoBehaviourPunCallbacks, MultiplayerAPI
{
    private event MultiplayerAPI.MultiplayerRoomAction onJoinedRoom;
    private event MultiplayerAPI.MultiplayerRoomAction onJoinRoomFailed;

    #region MonoBehaviour Methods
    public override void OnEnable()
    {
        base.OnEnable();

        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }
    #endregion

    #region MultiplayerAPI Implementation
    event MultiplayerAPI.MultiplayerRoomAction MultiplayerAPI.onJoinedRoom
    {
        add
        {
            onJoinedRoom += value;
        }

        remove
        {
            onJoinedRoom -= value;
        }
    }

    event MultiplayerAPI.MultiplayerRoomAction MultiplayerAPI.onJoinRoomFailed
    {
        add
        {
            onJoinRoomFailed += value;
        }

        remove
        {
            onJoinRoomFailed -= value;
        }
    }

    void MultiplayerAPI.Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void MultiplayerAPI.Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = PUNSettings.gameVersion;
    }

    void MultiplayerAPI.Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    bool MultiplayerAPI.IsConnected()
    {
        return PhotonNetwork.IsConnected;
    }

    void MultiplayerAPI.JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void MultiplayerAPI.CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = PUNSettings.maxPlayersPerRoom });
    }

    void MultiplayerAPI.LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    bool MultiplayerAPI.IsInRoom()
    {
        return PhotonNetwork.InRoom;
    }

    void MultiplayerAPI.LoadScene(int sceneIndex)
    {
        PhotonNetwork.LoadLevel(sceneIndex);
    }

    int MultiplayerAPI.PlayerCount()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount;
    }

    GameObject MultiplayerAPI.InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position)
    {
        GameObject playerGO = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
        return playerGO;
    }

    int MultiplayerAPI.GetNetworkIDByGameObject(GameObject go)
    {
        return go.GetPhotonView().ViewID;
    }

    GameObject MultiplayerAPI.GetGameObjectByNetworkID(int netID)
    {
        return PhotonNetwork.GetPhotonView(netID).gameObject;
    }

    void MultiplayerAPI.CommunicatePlayerReadyUpToServer(object[] data)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent((byte)NetworkEventHandler.NetworkEventCodes.onRemotePlayerReadyUp, data, raiseEventOptions, SendOptions.SendReliable);
    }
    #endregion

    #region PUN Callbacks
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        onJoinRoomFailed?.Invoke();
    }
    public override void OnJoinedRoom()
    {
        onJoinedRoom?.Invoke();
    }

    // temporarily here for testing purposes
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
    #endregion

    #region Custom Callbacks
    private void OnPhotonEvent(EventData photonEvent)
    {
        NetworkEventHandler.ProcessEvent(photonEvent.Code, (object[])photonEvent.CustomData);
    }
    #endregion
}
