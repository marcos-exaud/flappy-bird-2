using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PUNMultiplayerAPI : MonoBehaviourPunCallbacks, MultiplayerAPI
{
    private Dictionary<Photon.Realtime.Player, Player> playerDictionary;

    #region MultiplayerAPI Implementation
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = PUNSettings.gameVersion;
        }
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            LeaveRoom();
            PhotonNetwork.Disconnect();
        }
    }

    public void JoinRoom()
    {
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(expectedMaxPlayers: PUNSettings.maxPlayersPerRoom);
        }
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public Player InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position)
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
    #endregion

    /*
    private void JoinRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = null, byte expectedMaxPlayers = PUNSettings.maxPlayersPerRoom, string roomName = null, RoomOptions roomOptions = null)
    {
        PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: expectedCustomRoomProperties, expectedMaxPlayers: expectedMaxPlayers, roomName: roomName, roomOptions: roomOptions);
    }
    */
}
