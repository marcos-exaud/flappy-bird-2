using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    void IPlayerManager.RegisterPlayer(Player newPlayer)
    {
        if (!playerList.Contains(newPlayer))
        {
            playerList.Add(newPlayer);
        }
    }

    bool IPlayerManager.AllPlayersReady()
    {
        return !playerList.Any(player => player.GetReady() == false);
    }
}
