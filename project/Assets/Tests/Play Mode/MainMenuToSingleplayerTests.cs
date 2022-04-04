using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuToSingleplayerTests
{
    private const int DELAY_BETWEEN_SCENES = 1;

    // Menus
    private GameObject mainMenu;

    // Buttons
    private Button singleplayerButton;

    [OneTimeSetUp]
    public void Setup()
    {
        // Loads Main Menu
        SceneManager.LoadScene(Consts.STARTING_SCENE);
    }

    [UnityTest]
    public IEnumerator _1_loading_singleplayer()
    {
        // Arrange
        mainMenu = GameObject.Find("Default Menu");
        singleplayerButton = mainMenu.transform.Find("Singleplayer Button").gameObject.GetComponent<Button>();

        // Act
        singleplayerButton.onClick.Invoke();
        yield return new WaitForSeconds(DELAY_BETWEEN_SCENES);
        Scene singleplayerScene = SceneManager.GetSceneByBuildIndex(Consts.BASE_GAME_SCENE);

        // Assert
        Assert.AreEqual(true, singleplayerScene.isLoaded);
    }
}
