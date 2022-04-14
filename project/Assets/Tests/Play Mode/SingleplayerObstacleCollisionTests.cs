using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NSubstitute;
using UnityEngine.UI;

public class SingleplayerObstacleCollisionTests
{
    private const int DELAY_FOR_GAMEMANAGER_START = 1;
    private const float DELAY_BETWEEN_PLAYER_INPUT = 0.6f;

    // Game Objects
    private GameObject player;
    private GameObject gameManagerGameObject;

    // Game Object Components
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRigidbody2D;
    private GameManager gameManager;

    // Wrappers
    private InputWrapper inputWrapper;

    [OneTimeSetUp]
    public void Setup()
    {
        // Loads Singleplayer
        SceneManager.LoadScene(Consts.BASE_GAME_SCENE);
    }

    [UnityTest]
    public IEnumerator _1_collide_with_obstacle()
    {
        // Arrange
        inputWrapper = Substitute.For<InputWrapper>();
        inputWrapper.GetKeyDown(KeyCode.UpArrow).Returns<bool>(true);

        player = GameObject.Find("Player(Clone)");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerRigidbody2D = player.GetComponent<Rigidbody2D>();

        gameManagerGameObject = GameObject.Find("Game Manager");
        gameManager = gameManagerGameObject.GetComponent<GameManager>();

        bool eventRaised = false;
        System.Action EventRaised = new System.Action(() => { eventRaised = true; });
        EventManager.OnPlayerDeath += EventRaised;

        // Act
        yield return new WaitForSeconds(DELAY_FOR_GAMEMANAGER_START);
        ReflectionUtils.SetValue(gameManager, "inputWrapper", inputWrapper);
        for (int i = 0; i < 4; i++)
        {
            ReflectionUtils.Invoke(playerMovement, "Fly");
            yield return new WaitForSeconds(DELAY_BETWEEN_PLAYER_INPUT);
        }
        EventManager.OnPlayerDeath -= EventRaised;

        // Assert
        Assert.AreEqual(true, playerRigidbody2D.IsSleeping());
        Assert.AreEqual(false, playerMovement.enabled);
        Assert.AreEqual(true, eventRaised);
    }
}
