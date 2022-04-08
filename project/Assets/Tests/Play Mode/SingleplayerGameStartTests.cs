using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NSubstitute;
using System.Linq;

public class SingleplayerGameStartTests
{
    private const int DELAY_FOR_GAMEMANAGER_START = 1;
    private const int DELAY_FOR_PLAYERMANAGER_START = 1;

    private List<GameObjectWrapper> players;

    // Game Objects
    private GameObject player;
    private GameObject gameManagerGameObject;

    // Game Object Components
    private PlayerManager playerManager;
    private PlayerMovement playerMovement;
    private GameManager gameManager;

    // Rigidbodies
    private Rigidbody2D playerRigidbody;

    // Wrappers
    private InputWrapper inputWrapper;

    [OneTimeSetUp]
    public void Setup()
    {
        // Loads Singleplayer
        SceneManager.LoadScene(Consts.BASE_GAME_SCENE);
    }

    [UnityTest]
    public IEnumerator _1_instantiating_player()
    {
        // Arrange

        // Act
        yield return new WaitForSeconds(DELAY_FOR_GAMEMANAGER_START);
        player = GameObject.Find("Player(Clone)");
        playerManager = player.GetComponent<PlayerManager>();
        playerRigidbody = ReflectionUtils.GetValue<Rigidbody2D>(playerManager, "bird");
        players = PlayerList.players;

        // Assert
        // player was instantiated in scene
        Assert.AreNotEqual(null, player);

        yield return new WaitForSeconds(DELAY_FOR_PLAYERMANAGER_START);
        // player gameobject is in the correct state after instantiation
        Assert.AreNotEqual(null, playerRigidbody);
        Assert.AreEqual(true, playerRigidbody.IsSleeping());
        Assert.AreEqual(0, ReflectionUtils.GetValue<int>(playerManager, "playerScore"));

        // player was added to the player list
        Assert.AreNotEqual(null, (from gameObjectWrapper in players
                                  where gameObjectWrapper.gameObject.Equals(player)
                                  select gameObjectWrapper).First());
    }

    [UnityTest]
    public IEnumerator _2_player_ready_up()
    {
        // Arrange
        playerMovement = player.GetComponent<PlayerMovement>();

        inputWrapper = Substitute.For<InputWrapper>();
        inputWrapper.GetKeyDown(KeyCode.UpArrow).Returns<bool>(true);
        gameManagerGameObject = GameObject.Find("Game Manager");
        gameManager = gameManagerGameObject.GetComponent<GameManager>();

        List<GameObject> obstacles = ReflectionUtils.GetValue<List<GameObject>>(gameManager, "obstacles");
        List<ObstacleManager> obstacleManagers = new List<ObstacleManager>();
        obstacles.ForEach(x => obstacleManagers.Add(x.GetComponent<ObstacleManager>()));
        List<Rigidbody2D> obstacleRigidbodies = new List<Rigidbody2D>();
        obstacleManagers.ForEach(x => obstacleRigidbodies.Add(ReflectionUtils.GetValue<Rigidbody2D>(x, "obstacle")));

        // Act
        ReflectionUtils.SetValue(gameManager, "inputWrapper", inputWrapper);
        ReflectionUtils.Invoke(playerMovement, "Fly");
        yield return null;

        // Assert
        Assert.AreEqual(false, playerRigidbody.IsSleeping());
        obstacleRigidbodies.ForEach(x =>
        {
            Assert.AreEqual(false, x.IsSleeping());
            Assert.AreEqual(new Vector2(-Consts.GAME_X_SCROLLING_SPEED, 0f), x.velocity);
        });
    }
}
