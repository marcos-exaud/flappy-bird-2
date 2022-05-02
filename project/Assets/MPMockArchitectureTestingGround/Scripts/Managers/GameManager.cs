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
            InstantiateLocalPlayer(playerManager.GetPlayerPrefab(), new Vector2(Consts.DEFAULT_PLAYER_POSITION, Consts.DEFAULT_PLAYER_ALTITUDE));
        }

        StartCoroutine(StartGame());
    }

    void OnEnable()
    {
        Player.onLocalPlayerReadyUp += CommunicatePlayerReadyUpToServer;
        NetworkEventHandler.onRemotePlayerReadyUp += UpdateRemotePlayerReadyState;
    }

    void OnDisable()
    {
        Player.onLocalPlayerReadyUp -= CommunicatePlayerReadyUpToServer;
    }

    void OnDestroy()
    {
        Player.onLocalPlayerReadyUp -= CommunicatePlayerReadyUpToServer;
    }
    #endregion

    private IEnumerator StartGame()
    {
        yield return new WaitUntil(() => playerManager.AllPlayersReady());

        // Start the game
        Debug.Log("Game Started!");
    }

    private void InstantiateLocalPlayer(GameObject playerPrefab, Vector2 position)
    {
        if (playerPrefab != null && position != null)
        {
            GameObject playerGO = mpAPI.InstantiateLocalPlayer(playerManager.GetPlayerPrefab(), new Vector2(Consts.DEFAULT_PLAYER_POSITION, Consts.DEFAULT_PLAYER_ALTITUDE));
            Player player = playerGO.GetComponent<Player>();
            playerManager.SetLocalPlayer(player);
        }
    }

    private void CommunicatePlayerReadyUpToServer(Player player)
    {
        int playerNetID = mpAPI.GetNetworkIDByGameObject(player.gameObject);
        mpAPI.CommunicatePlayerReadyUpToServer(new object[] { playerNetID });
    }

    private void UpdateRemotePlayerReadyState(int playerNetID)
    {
        try
        {
            GameObject playerGO = mpAPI.GetGameObjectByNetworkID(playerNetID);
            Player player = playerGO.GetComponent<Player>();
            player.SetReady(true);
        }
        catch (System.Exception)
        {
            // The player in question probably left the room during this operation
            throw;
        }
    }

    public void Disconnect()
    {
        if (mpAPI.IsConnected())
        {
            if (mpAPI.IsInRoom())
            {
                mpAPI.LeaveRoom();
            }
            mpAPI.Disconnect();
        }
    }
}
