using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreboard;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject multiplayerMenuUI;

    [SerializeField]
    private GameObject defaultMenuUI;

    void OnEnable()
    {
        EventManager.OnGameOver += DisplayGameOverUI;
    }

    void OnDisable()
    {
        EventManager.OnGameOver -= DisplayGameOverUI;
    }

    void OnDestroy()
    {
        EventManager.OnGameOver -= DisplayGameOverUI;
    }

    public void UpdateScoreboard(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        int clearerScore = playerManager.GetPlayerScore();

        scoreboard.text = "" + clearerScore;
    }

    // display game over screen
    private void DisplayGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    /// <summary>
    /// Method <c>ToggleMainMenuUI</c> Toggles between the Default Menu UI and the Multiplayer Menu UI in the Main Menu Screen
    /// </summary>
    public void ToggleMainMenuUI()
    {
        bool multiplayerUIIsToggled = multiplayerMenuUI.activeSelf;
        multiplayerMenuUI.SetActive(!multiplayerUIIsToggled);
        defaultMenuUI.SetActive(multiplayerUIIsToggled);

    }
}
