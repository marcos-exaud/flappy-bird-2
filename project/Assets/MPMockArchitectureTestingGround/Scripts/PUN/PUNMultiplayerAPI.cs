using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class PUNMultiplayerAPI : MonoBehaviourPunCallbacks, MultiplayerAPI
{
    private Dictionary<Photon.Realtime.Player, Player> playerDictionary;

    #region MultiplayerAPI Implementation
    void MultiplayerAPI.Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void MultiplayerAPI.Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = PUNSettings.gameVersion;
        }
    }

    void MultiplayerAPI.Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            ((MultiplayerAPI)this).LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }

    void MultiplayerAPI.JoinRoom()
    {
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    void MultiplayerAPI.LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    Player MultiplayerAPI.InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position)
    {
        if (playerPrefab != null && position != null)
        {
            GameObject playerGO = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
            return playerGO.GetComponent<Player>();
        }
        return null;
    }

    void MultiplayerAPI.RegisterPlayerOnNetwork(Player player)
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
    }
    #endregion

    #region PUN Callbacks
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = PUNSettings.maxPlayersPerRoom });
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
    #endregion
}
