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
            InstantiateLocalPlayer(playerManager.GetPlayerPrefab().GameObject, new Vector2(Consts.DEFAULT_PLAYER_POSITION, Consts.DEFAULT_PLAYER_ALTITUDE));
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
        NetworkEventHandler.onRemotePlayerReadyUp -= UpdateRemotePlayerReadyState;

    }

    void OnDestroy()
    {
        Player.onLocalPlayerReadyUp -= CommunicatePlayerReadyUpToServer;
        NetworkEventHandler.onRemotePlayerReadyUp -= UpdateRemotePlayerReadyState;

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
            WGameObject playerGO = new WGameObject(mpAPI.InstantiateLocalPlayer(playerPrefab, new Vector2(Consts.DEFAULT_PLAYER_POSITION, Consts.DEFAULT_PLAYER_ALTITUDE)));
            IPlayer player = playerGO.GetComponent<IPlayer>();
            playerManager.SetLocalPlayer(player);
        }
    }

    private void CommunicatePlayerReadyUpToServer(IPlayer player)
    {
        int playerNetID = mpAPI.GetNetworkIDByGameObject(player.gameObject.GameObject);
        mpAPI.CommunicatePlayerReadyUpToServer(new object[] { playerNetID });
    }

    private void UpdateRemotePlayerReadyState(int playerNetID)
    {
        try
        {
            WGameObject playerGO = new WGameObject(mpAPI.GetGameObjectByNetworkID(playerNetID));
            IPlayer player = playerGO.GetComponent<IPlayer>();
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
