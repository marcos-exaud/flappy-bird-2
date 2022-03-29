using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerGameManager : GameManager
{
    [HideInInspector]
    public static bool gameIsRunning;
    protected override void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerManagerMultiplayer.localPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(-5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        gameIsRunning = false;
        StartCoroutine(StartGame());
    }

    public override void OnEnable()
    {
        base.OnEnable();

        EventManager.OnGameOver += StopGame;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        EventManager.OnGameOver -= StopGame;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnGameOver -= StopGame;
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(Consts.STARTING_SCENE);
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadArena();
        }
    }

    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel(Consts.MULTIPLAYER_GAME_SCENE_1P);
    }

    protected override IEnumerator StartGame()
    {
        yield return new WaitUntil(() => AllPlayersReady());

        Debug.Log("all players are ready");

        // wake up all players and obstacles to properly start the game
        foreach (GameObject player in PlayerList.players)
        {
            player.GetComponent<PlayerManager>().WakeUp();
        }
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<ObstacleManager>().WakeUp();
        }

        gameIsRunning = true;
    }

    private bool AllPlayersReady()
    {
        foreach (GameObject player in PlayerList.players)
        {
            PlayerManagerMultiplayer playerManager = player.GetComponent<PlayerManagerMultiplayer>();
            if (!playerManager.ready) return false;
        }
        return true;
    }

    // clearer is the player who cleared the obstacle
    protected override void IncrementPlayerScore(GameObject obstacleClearer)
    {
        GameObject player = PlayerList.players.Find((player) => player.name == obstacleClearer.name);

        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.IncrementScore();

        uiManager.GetComponent<UIManager>().UpdateScoreboard(player);
    }

    protected override void CheckForGameOver()
    {
        // if any player is still alive, the game is not over
        foreach (GameObject player in PlayerList.players)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager.PlayerIsAlive()) return;
        }

        // otherwise, invoke the game over event
        EventManager.OnGameOver?.Invoke();
    }

    private void StopGame()
    {
        gameIsRunning = false;
    }
}
