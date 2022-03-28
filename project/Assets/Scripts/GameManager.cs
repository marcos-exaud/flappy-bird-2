using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> players;

    [SerializeField]
    private List<GameObject> obstacles;

    [SerializeField]
    private GameObject uiManager;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    public override void OnEnable()
    {
        base.OnEnable();

        EventManager.OnObstacleClear += IncrementPlayerScore;
        EventManager.OnPlayerDeath += CheckForGameOver;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        EventManager.OnObstacleClear -= IncrementPlayerScore;
        EventManager.OnPlayerDeath -= CheckForGameOver;
    }

    void OnDestroy()
    {
        EventManager.OnObstacleClear -= IncrementPlayerScore;
        EventManager.OnPlayerDeath -= CheckForGameOver;
    }

    private IEnumerator StartGame()
    {
        // wait for player to input first movement to start the game
        yield return new WaitUntil( () => Input.GetKeyDown(KeyCode.UpArrow));

        // wake up all players and obstacles to properly start the game
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerManager>().WakeUp();
        }
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<ObstacleManager>().WakeUp();
        }
    }

    // clearer is the player who cleared the obstacle
    private void IncrementPlayerScore(GameObject obstacleClearer)
    {
        GameObject player = players.Find((player) => player.name == obstacleClearer.name);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.IncrementScore();

        uiManager.GetComponent<UIManager>().UpdateScoreboard(player);
    }

    private void CheckForGameOver()
    {
        // if any player is still alive, the game is not over
        foreach (GameObject player in players)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager.PlayerIsAlive()) return;
        }

        // otherwise, invoke the game over event
        EventManager.OnGameOver?.Invoke();
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel(Consts.MULTIPLAYER_GAME_SCENE_1P);
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
}
