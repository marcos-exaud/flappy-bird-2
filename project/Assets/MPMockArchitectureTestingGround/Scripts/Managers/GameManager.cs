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
        playerManager = gameObject.GetComponent<IPlayerManager>();
        if (playerManager.localPlayer == null)
        {
            Player player = mpAPI.InstantiateLocalPlayer(playerManager.GetPlayerPrefab(), new Vector2(Consts.DEFAULT_PLAYER_POSITION, Consts.DEFAULT_PLAYER_ALTITUDE));
            playerManager.SetLocalPlayer(player);
        }
    }

    void OnEnable()
    {
        Player.onPlayerStart += mpAPI.RegisterPlayerOnNetwork;
        Player.onPlayerDestroy += mpAPI.UnregisterPlayerOnNetwork;
    }

    void OnDisable()
    {
        Player.onPlayerStart -= mpAPI.RegisterPlayerOnNetwork;
        Player.onPlayerDestroy -= mpAPI.UnregisterPlayerOnNetwork;
    }

    void OnDestroy()
    {
        Player.onPlayerStart -= mpAPI.RegisterPlayerOnNetwork;
        Player.onPlayerDestroy -= mpAPI.UnregisterPlayerOnNetwork;
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
