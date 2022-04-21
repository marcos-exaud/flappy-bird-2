using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerManager
{
    public Player localPlayer { get; }

    #region Getters and Setters
    public GameObject GetPlayerPrefab();
    public void SetLocalPlayer(Player player);
    #endregion

    public bool AllPlayersReady();
}
