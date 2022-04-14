using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NSubstitute;
using UnityEngine.UI;

public class SingleplayerToMainMenuTests
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
    private Button quitToMainMenuButton;

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
        //gameOverUI = ReflectionUtils.GetValue<GameObjectWrapper>(uiManagerGameObject, "gameOverUIWrapper").gameObject;
        gameOverUI = GameObject.Find("Game Over Canvas");

        // Assert
        Assert.AreEqual(true, gameOverUI.activeInHierarchy);
    }

    [UnityTest]
    public IEnumerator _2_quitting_to_main_menu()
    {
        // Arrange
        gameOverMenu = GameObject.Find("Game Over Canvas");
        quitToMainMenuButton = gameOverMenu.transform.Find("Quit To Main Menu Button").gameObject.GetComponent<Button>();

        // Act
        quitToMainMenuButton.onClick.Invoke();
        yield return new WaitForSeconds(DELAY_BETWEEN_SCENES);
        Scene mainMenuScene = SceneManager.GetSceneByBuildIndex(Consts.STARTING_SCENE);

        // Assert
        Assert.AreEqual(true, mainMenuScene.isLoaded);
    }
}
