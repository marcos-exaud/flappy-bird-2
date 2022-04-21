using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MultiplayerAPI
{
    public void Start();
    
    public void Connect();

    public void Disconnect();

    public void JoinRoom();

    public void LeaveRoom();

    public Player InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position);

    public void RegisterPlayerOnNetwork(Player player);

    public void UnregisterPlayerOnNetwork(Player player);
}
