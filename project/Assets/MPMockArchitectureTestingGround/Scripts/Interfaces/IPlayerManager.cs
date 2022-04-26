using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerManager
{
    public IPlayer localPlayer { get; }

    #region Getters and Setters
    public GameObject GetPlayerPrefab();
    public void SetLocalPlayer(IPlayer player);
    #endregion

    public bool AllPlayersReady();
}
