using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MultiplayerAPI
{
    public void Start();
    
    public void Connect();
    public void Disconnect();
    public bool IsConnected();

    public void JoinRoom();
    public void CreateRoom();
    public void LeaveRoom();
    public bool IsInRoom();
    public delegate void MultiplayerRoomAction();
    public event MultiplayerRoomAction onJoinedRoom;
    public event MultiplayerRoomAction onJoinRoomFailed;

    public void LoadScene(int sceneIndex);

    public int PlayerCount();

    public GameObject InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position);

    public int GetNetworkIDByGameObject(GameObject go);

    public GameObject GetGameObjectByNetworkID(int netID);

    public void CommunicatePlayerReadyUpToServer(object[] data);
}
