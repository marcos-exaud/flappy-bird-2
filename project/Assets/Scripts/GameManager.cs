using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    void OnEnable()
    {
        EventManager.OnObstacleClear += IncrementPlayerScore;
    }

    void OnDisable()
    {
        EventManager.OnObstacleClear -= IncrementPlayerScore;
    }

    void OnDestroy()
    {
        EventManager.OnObstacleClear -= IncrementPlayerScore;
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
    public void IncrementPlayerScore(GameObject obstacleClearer)
    {
        GameObject player = players.Find((player) => player.name == obstacleClearer.name);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.IncrementScore();

        uiManager.GetComponent<UIManager>().UpdateScoreboard(player);
    }
}
