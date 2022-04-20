using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

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
            PhotonNetwork.JoinRandomOrCreateRoom(expectedMaxPlayers: PUNSettings.maxPlayersPerRoom);
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
    #endregion

    #region PUN Callbacks
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel(5);
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(4);
    }
    #endregion

    /*
    private void JoinRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = null, byte expectedMaxPlayers = PUNSettings.maxPlayersPerRoom, string roomName = null, RoomOptions roomOptions = null)
    {
        PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: expectedCustomRoomProperties, expectedMaxPlayers: expectedMaxPlayers, roomName: roomName, roomOptions: roomOptions);
    }
    */
}
