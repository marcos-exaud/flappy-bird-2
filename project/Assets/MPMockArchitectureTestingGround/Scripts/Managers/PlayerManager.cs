using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IPlayerManager
{
    private Player localPlayer { get; set; }
    Player IPlayerManager.localPlayer { get { return localPlayer; } }
    private List<Player> playerList;

    // Prefabs
    [SerializeField]
    private GameObject playerPrefab;

    #region MonoBehaviour Methods
    void Awake()
    {
        localPlayer = null;
        playerList = new List<Player>();
    }

    void OnEnable()
    {
        Player.onPlayerStart += RegisterPlayer;
        Player.onPlayerStart += HandleRemotePlayer;
        Player.onPlayerDestroy += UnregisterPlayer;
    }

    void OnDisable()
    {
        Player.onPlayerStart -= RegisterPlayer;
        Player.onPlayerStart -= HandleRemotePlayer;
        Player.onPlayerDestroy -= UnregisterPlayer;
    }

    void OnDestroy()
    {
        Player.onPlayerStart -= RegisterPlayer;
        Player.onPlayerStart -= HandleRemotePlayer;
        Player.onPlayerDestroy -= UnregisterPlayer;
    }
    #endregion

    #region Getters and Setters
    GameObject IPlayerManager.GetPlayerPrefab()
    {
        return playerPrefab;
    }

    void IPlayerManager.SetLocalPlayer(Player player)
    {
        localPlayer = player;
    }
    #endregion

    private void RegisterPlayer(Player newPlayer)
    {
        if (!playerList.Contains(newPlayer))
        {
            playerList.Add(newPlayer);
        }
    }

    private void UnregisterPlayer(Player newPlayer)
    {
        if (playerList.Contains(newPlayer))
        {
            playerList.Remove(newPlayer);
        }
    }

    private void HandleRemotePlayer(Player player)
    {
        if (player != localPlayer)
        {
            player.ChangeSprite();
            player.TogglePhysics();
        }
    }

    bool IPlayerManager.AllPlayersReady()
    {
        if (playerList.Any())
        {
            return !playerList.Any(player => player.GetReady() == false);
        }
        return false;
    }
}
