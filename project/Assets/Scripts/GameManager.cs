using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    [SerializeField]
    protected List<GameObject> obstacles;

    [SerializeField]
    protected GameObject uiManager;

    // Wrappers
    protected InputWrapper inputWrapper;
    protected GameObjectWrapper uiManagerWrapper;

    protected virtual void Start()
    {
        GameObject player = GameObject.Instantiate(playerPrefab, new Vector2(-5f, 0f), Quaternion.identity);

        InitWrappers();

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

    protected virtual void OnDestroy()
    {
        EventManager.OnObstacleClear -= IncrementPlayerScore;
        EventManager.OnPlayerDeath -= CheckForGameOver;
    }

    protected virtual void InitWrappers()
    {
        if (inputWrapper == null) inputWrapper = new InputWrapper();
        uiManagerWrapper = new GameObjectWrapper(uiManager);
    }

    protected virtual IEnumerator StartGame()
    {
        // wait for player to input first movement to start the game
        yield return new WaitUntil(() => inputWrapper.GetKeyDown(KeyCode.UpArrow));

        // wake up all players and obstacles to properly start the game
        foreach (GameObjectWrapper player in PlayerList.players)
        {
            player.GetComponent<PlayerManager>().WakeUp();
        }
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<ObstacleManager>().WakeUp();
        }
    }

    // clearer is the player who cleared the obstacle
    protected virtual void IncrementPlayerScore(GameObjectWrapper obstacleClearer)
    {
        GameObjectWrapper player = PlayerList.players.Find((player) => player.gameObject.name == obstacleClearer.gameObject.name);
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        playerManager.IncrementScore();

        uiManagerWrapper.GetComponent<UIManager>().UpdateScoreboard(player);
    }

    protected virtual void CheckForGameOver()
    {
        // if any player is still alive, the game is not over
        foreach (GameObjectWrapper player in PlayerList.players)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager.PlayerIsAlive()) return;
        }

        // otherwise, invoke the game over event
        EventManager.OnGameOver?.Invoke();
    }
}
