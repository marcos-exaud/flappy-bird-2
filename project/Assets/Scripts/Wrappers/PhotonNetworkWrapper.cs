using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkWrapper
{
    public PhotonNetworkWrapper() { }

    public virtual bool IsConnected
    {
        get { return PhotonNetwork.IsConnected; }
    }

    public virtual bool AutomaticallySyncScene
    {
        get { return PhotonNetwork.AutomaticallySyncScene; }
        set { PhotonNetwork.AutomaticallySyncScene = value; }
    }

    public virtual Room CurrentRoom
    {
        get { return PhotonNetwork.CurrentRoom; }
    }

    public virtual string GameVersion
    {
        get { return PhotonNetwork.GameVersion; }
        set { PhotonNetwork.GameVersion = value; }
    }

    public virtual bool JoinRandomRoom()
    {
        return PhotonNetwork.JoinRandomRoom();
    }

    public virtual bool CreateRoom(string roomName, RoomOptions roomOptions = null, TypedLobby typedLobby = null, string[] expectedUsers = null)
    {
        return PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby, expectedUsers);
    }

    public virtual void LoadLevel(int levelNumber)
    {
        PhotonNetwork.LoadLevel(levelNumber);
    }

    public virtual bool ConnectUsingSettings()
    {
        return PhotonNetwork.ConnectUsingSettings();
    }
}