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

    public Player InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position);

    /*public void RegisterPlayerOnNetwork(Player player);
    public void UnregisterPlayerOnNetwork(Player player);*/
}
