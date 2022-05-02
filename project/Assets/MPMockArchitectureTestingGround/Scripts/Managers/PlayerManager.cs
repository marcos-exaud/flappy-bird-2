using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IPlayerManager
{
    private IPlayer localPlayer { get; set; }
    IPlayer IPlayerManager.localPlayer { get { return localPlayer; } }
    private List<IPlayer> playerList;

    // Prefabs
    [SerializeField]
    private WGameObject playerPrefab;

    #region MonoBehaviour Methods
    void Awake()
    {
        localPlayer = null;
        playerList = new List<IPlayer>();
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
    WGameObject IPlayerManager.GetPlayerPrefab()
    {
        return playerPrefab;
    }

    void IPlayerManager.SetLocalPlayer(IPlayer player)
    {
        localPlayer = player;
    }
    #endregion

    private void RegisterPlayer(IPlayer newPlayer)
    {
        if (!playerList.Contains(newPlayer))
        {
            playerList.Add(newPlayer);
        }
    }

    private void UnregisterPlayer(IPlayer newPlayer)
    {
        if (playerList.Contains(newPlayer))
        {
            playerList.Remove(newPlayer);
        }
    }

    private void HandleRemotePlayer(IPlayer player)
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
