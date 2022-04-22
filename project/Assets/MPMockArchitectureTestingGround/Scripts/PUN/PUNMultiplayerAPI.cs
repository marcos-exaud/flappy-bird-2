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
    private Dictionary<Photon.Realtime.Player, Player> playerDictionary = new Dictionary<Photon.Realtime.Player, Player>();

    private event MultiplayerAPI.MultiplayerRoomAction onJoinedRoom;
    private event MultiplayerAPI.MultiplayerRoomAction onJoinRoomFailed;

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

    Player MultiplayerAPI.InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position)
    {
        GameObject playerGO = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
        return playerGO.GetComponent<Player>();
    }

    /*void MultiplayerAPI.RegisterPlayerOnNetwork(Player player)
    {
        PhotonView playerPhotonView = player.gameObject.GetPhotonView();
        Photon.Realtime.Player punPlayer = playerPhotonView.Owner;

        if (playerDictionary.ContainsKey(punPlayer))
        {
            playerDictionary[punPlayer] = player;
        }
        else
        {
            playerDictionary.Add(punPlayer, player);
        }
    }

    void MultiplayerAPI.UnregisterPlayerOnNetwork(Player player)
    {
        PhotonView playerPhotonView = player.gameObject.GetPhotonView();
        Photon.Realtime.Player punPlayer = playerPhotonView.Owner;

        playerDictionary.Remove(punPlayer);
    }*/
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
}
