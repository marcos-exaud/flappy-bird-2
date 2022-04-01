using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerGameManager : GameManager
{
    [HideInInspector]
    public static bool gameIsRunning = false;

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
                player.GetPhotonView().RPC("DisplayNickname", RpcTarget.Others);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }

        Hashtable hash = new Hashtable();
        hash.Add("Rematch", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        hash = new Hashtable();
        hash.Add("Score", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        gameIsRunning = false;
        StartCoroutine(StartGame());
    }

    void Update()
    {
        // sanity check in case game over isnt detected correctly
        if (gameIsRunning && PhotonNetwork.IsMasterClient && PlayerList.players.Count == 0) CheckForGameOver();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    public void OnPhotonEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        switch (eventCode)
        {
            case EventManager.OnGameOverPhotonEventCode:
                StopGame();
                break;
            case EventManager.OnPlayerDeathPhotonEventCode:
                CheckForGameOver();
                break;
        }
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
        DestroyAllPlayerGameObjects();
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel(MultiplayerGameManager.GetNextMultiplayerScene());
    }

    protected override IEnumerator StartGame()
    {
        yield return new WaitUntil(() => AllPlayersReady());

        yield return uiManager.GetComponent<MultiplayerUIManager>().Countdown();

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
        List<GameObject> players = PlayerList.players;

        // players cant be ready if there arent any
        if (players.Count == 0) return false;

        foreach (GameObject player in players)
        {
            PlayerManagerMultiplayer playerManager = player.GetComponent<PlayerManagerMultiplayer>();
            if (!playerManager.ready) return false;
        }
        return true;
    }

    // clearer is the player who cleared the obstacle
    protected override void IncrementPlayerScore(GameObject obstacleClearer)
    {
        if (obstacleClearer.Equals(PlayerManagerMultiplayer.localPlayerInstance))
        {
            PlayerManager playerManager = obstacleClearer.GetComponent<PlayerManager>();
            playerManager.IncrementScore();

            uiManager.GetComponent<UIManager>().UpdateScoreboard(obstacleClearer);
        }
    }

    protected override void CheckForGameOver()
    {
        // players are destroyed on death, so there must be no players left before triggering game over
        if (PlayerList.players.Count > 0) return;

        // otherwise, invoke the game over event
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(EventManager.OnGameOverPhotonEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    private void StopGame()
    {
        gameIsRunning = false;
    }

    public static void DestroyAllPlayerGameObjects()
    {
        List<GameObject> players = PlayerList.players;
        foreach (GameObject player in players)
        {
            player.GetPhotonView().RPC("DestroyPlayerGameObject", RpcTarget.All);
        }
    }

    /// <summary>
    /// Method <c>GetNextMultiplayerScene</c> Serves as a workaround to a problem with photon's LoadLevelIfSynced when trying to
    /// load the same scene by alternating between two identical multiplayer scenes.
    /// </summary>
    public static int GetNextMultiplayerScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == Consts.MULTIPLAYER_GAME_SCENE_2P)
        {
            return Consts.MULTIPLAYER_GAME_SCENE_1P;
        }
        else
        {
            return Consts.MULTIPLAYER_GAME_SCENE_2P;
        }
    }
}
