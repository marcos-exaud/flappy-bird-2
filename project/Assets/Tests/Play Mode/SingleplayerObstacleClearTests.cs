using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NSubstitute;
using UnityEngine.UI;
using TMPro;

public class SingleplayerObstacleClearTests
{
    private const int DELAY_FOR_GAMEMANAGER_START = 1;
    private const float DELAY_BETWEEN_PLAYER_INPUT = 0.65f;

    // Game Objects
    private GameObject player;
    private GameObject gameManagerGameObject;

    // Game Object Components
    private PlayerMovement playerMovement;
    private GameManager gameManager;
    private PlayerManager playerManager;

    // Labels
    private TextMeshProUGUI scoreDisplay;

    // Wrappers
    private InputWrapper inputWrapper;

    [OneTimeSetUp]
    public void Setup()
    {
        // Loads Singleplayer
        SceneManager.LoadScene(Consts.BASE_GAME_SCENE);
    }

    [UnityTest]
    public IEnumerator _1_clear_obstacle()
    {
        // Arrange
        inputWrapper = Substitute.For<InputWrapper>();
        inputWrapper.GetKeyDown(KeyCode.UpArrow).Returns<bool>(true);

        player = GameObject.Find("Player(Clone)");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerManager = player.GetComponent<PlayerManager>();

        gameManagerGameObject = GameObject.Find("Game Manager");
        gameManager = gameManagerGameObject.GetComponent<GameManager>();

        scoreDisplay = GameObject.Find("Score Display").GetComponent<TextMeshProUGUI>();

        bool eventRaised = false;
        System.Action<GameObjectWrapper> EventRaised = new System.Action<GameObjectWrapper>((x) => { if (x.gameObject.Equals(player)) eventRaised = true; });
        EventManager.OnObstacleClear += EventRaised;

        // Act
        yield return new WaitForSeconds(DELAY_FOR_GAMEMANAGER_START);
        ReflectionUtils.SetValue(gameManager, "inputWrapper", inputWrapper);
        for (int i = 0; i < 4; i++)
        {
            ReflectionUtils.Invoke(playerMovement, "Fly");
            yield return new WaitForSeconds(DELAY_BETWEEN_PLAYER_INPUT);
        }
        EventManager.OnObstacleClear -= EventRaised;

        // Assert
        Assert.AreEqual(true, eventRaised);
        Assert.AreEqual(true, playerManager.GetPlayerScore() == 1);
        Assert.AreEqual(true, scoreDisplay.text == "1");
    }
}
