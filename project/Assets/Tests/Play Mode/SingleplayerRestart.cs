using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NSubstitute;
using UnityEngine.UI;

public class SingleplayerRestart
{
    private const int DELAY_FOR_GAMEMANAGER_START = 1;
    private const int DELAY_FOR_PLAYER_DEATH = 3;
    private const int DELAY_BETWEEN_SCENES = 1;

    // Game Objects
    private GameObject player;
    private GameObject gameManagerGameObject;
    private GameObject uiManagerGameObject;
    private GameObject gameOverUI;

    // Game Object Components
    private GameManager gameManager;
    private PlayerMovement playerMovement;

    // Wrappers
    private InputWrapper inputWrapper;

    // Menus
    private GameObject gameOverMenu;

    // Buttons
    private Button restartButton;

    [OneTimeSetUp]
    public void Setup()
    {
        // Loads Singleplayer
        SceneManager.LoadScene(Consts.BASE_GAME_SCENE);
    }

    [UnityTest]
    public IEnumerator _1_trigger_game_over()
    {
        // Arrange
        inputWrapper = Substitute.For<InputWrapper>();
        inputWrapper.GetKeyDown(KeyCode.UpArrow).Returns<bool>(true);

        gameManagerGameObject = GameObject.Find("Game Manager");
        gameManager = gameManagerGameObject.GetComponent<GameManager>();

        uiManagerGameObject = GameObject.Find("UI Manager");

        player = GameObject.Find("Player(Clone)");
        playerMovement = player.GetComponent<PlayerMovement>();

        // Act
        yield return new WaitForSeconds(DELAY_FOR_GAMEMANAGER_START);
        ReflectionUtils.SetValue(gameManager, "inputWrapper", inputWrapper);
        yield return new WaitForSeconds(DELAY_FOR_PLAYER_DEATH);
        gameOverUI = GameObject.Find("Game Over Canvas");

        // Assert
        Assert.AreEqual(true, gameOverUI.activeInHierarchy);
    }

    [UnityTest]
    public IEnumerator _2_restarting_game()
    {
        // Arrange
        gameOverMenu = GameObject.Find("Game Over Canvas");
        restartButton = gameOverMenu.transform.Find("Restart Button").gameObject.GetComponent<Button>();

        bool eventRaised = false;
        UnityEngine.Events.UnityAction<Scene, Scene> EventRaised = new UnityEngine.Events.UnityAction<Scene, Scene>((x, y) => { eventRaised = true; });
        SceneManager.activeSceneChanged += EventRaised;

        // Act
        restartButton.onClick.Invoke();
        yield return new WaitForSeconds(DELAY_BETWEEN_SCENES);
        Scene singleplayerScene = SceneManager.GetSceneByBuildIndex(Consts.BASE_GAME_SCENE);
        SceneManager.activeSceneChanged -= EventRaised;

        // Assert
        Assert.AreEqual(true, eventRaised);
        Assert.AreEqual(true, singleplayerScene.isLoaded);
    }
}
