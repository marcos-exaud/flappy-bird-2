using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MainMenuToMultiplayerTests
{
    private const int DELAY_BETWEEN_SCENES = 2;
    private const int DELAY_BETWEEN_MENUS = 1;
    private const int DELAY_FROM_INPUT_FIELD_USE = 1;
    private const int DELAY_FROM_PHOTON_CONNECTION = 2;
    private const string PLAYER_NICK = "MiwtreTest";

    // Menus
    private GameObject mainMenu;
    private GameObject multiplayerMenu;
    private GameObject menuPanel;

    // Buttons
    private Button multiplayerMenuButton;
    private Button connectButton;

    // Input Fields
    private TMP_InputField playerNameInput;

    // Labels
    private TextMeshProUGUI progressLabel;

    [OneTimeSetUp]
    public void Setup()
    {
        // Loads Main Menu
        SceneManager.LoadScene(Consts.STARTING_SCENE);
    }

    [UnityTest]
    public IEnumerator _1_toggling_multiplayer_menu()
    {
        // Arrange
        mainMenu = GameObject.Find("Default Menu");
        multiplayerMenuButton = mainMenu.transform.Find("Multiplayer Button").gameObject.GetComponent<Button>();

        // Act
        multiplayerMenuButton.onClick.Invoke();
        yield return new WaitForSeconds(DELAY_BETWEEN_MENUS);
        multiplayerMenu = GameObject.Find("Multiplayer Menu");

        // Assert
        Assert.AreEqual(false, mainMenu.activeInHierarchy);
        Assert.AreEqual(true, multiplayerMenu.activeInHierarchy);
    }

    [UnityTest]
    public IEnumerator _2_1_connecting_to_room()
    {
        // Arrange
        playerNameInput = multiplayerMenu.transform.Find("Player Name Input").GetComponent<TMP_InputField>();

        // Act
        playerNameInput.text = PLAYER_NICK;
        yield return new WaitForSeconds(DELAY_FROM_INPUT_FIELD_USE);

        // Assert
        Assert.AreEqual(PLAYER_NICK, PlayerPrefs.GetString("PlayerName"));
        Assert.AreEqual(PLAYER_NICK, PhotonNetwork.NickName);
    }

    [UnityTest]
    public IEnumerator _2_2_connecting_to_room()
    {
        // Arrange
        connectButton = multiplayerMenu.transform.Find("Connect Button").GetComponent<Button>();
        menuPanel = GameObject.Find("Menu Panel");

        // Act
        connectButton.onClick.Invoke();
        yield return new WaitForSeconds(DELAY_BETWEEN_MENUS);
        progressLabel = GameObject.Find("Progress Label").GetComponent<TextMeshProUGUI>();

        // Assert
        Assert.AreEqual(false, menuPanel.activeInHierarchy);
        Assert.AreEqual(true, progressLabel.IsActive());

        yield return new WaitForSeconds(DELAY_FROM_PHOTON_CONNECTION);
        Assert.AreEqual(true, PhotonNetwork.IsConnected);
        Assert.AreEqual(true, PhotonNetwork.InRoom);

        yield return new WaitForSeconds(DELAY_BETWEEN_SCENES);
        Assert.AreEqual(Consts.MULTIPLAYER_GAME_SCENE_1P, SceneManager.GetActiveScene().buildIndex);
        PhotonNetwork.Disconnect();
    }
}
