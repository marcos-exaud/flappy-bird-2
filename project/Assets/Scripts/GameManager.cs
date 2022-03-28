using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    protected static List<GameObject> players;

    [SerializeField]
    private List<GameObject> obstacles;

    [SerializeField]
    private GameObject uiManager;

    protected virtual void Start()
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
}
