using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // APIs
    private MultiplayerAPI mpAPI;

    // Other Managers
    private IPlayerManager playerManager;

    #region MonoBehaviour Methods
    void Awake()
    {
        mpAPI = gameObject.GetComponent<MultiplayerAPI>();
    }

    void Start()
    {
        playerManager = gameObject.GetComponent<PlayerManager>();
        if (playerManager.localPlayer == null)
        {
            Player player = mpAPI.InstantiateLocalPlayer(playerManager.GetPlayerPrefab(), new Vector2(Consts.DEFAULT_PLAYER_POSITION, Consts.DEFAULT_PLAYER_ALTITUDE));
            playerManager.SetLocalPlayer(player);
        }
    }
    #endregion

    private IEnumerator StartGame()
    {
        yield return new WaitUntil(() => playerManager.AllPlayersReady());

        // Start the game
    }

    public void Disconnect()
    {
        mpAPI.Disconnect();
    }
}
