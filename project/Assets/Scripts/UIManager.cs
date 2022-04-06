using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Ui Text box for displaying teh score")]
    [SerializeField]
    private TextMeshProUGUI scoreboard;

    [Tooltip("The Ui Panel containing the game over menu")]
    [SerializeField]
    protected GameObject gameOverUI;

    [Tooltip("The Ui Panel containing the multiplayer menu")]
    [SerializeField]
    private GameObject multiplayerMenuUI;

    [Tooltip("The Ui Panel containing the default menu")]
    [SerializeField]
    private GameObject defaultMenuUI;

    [Tooltip("The Ui Panel containing the default menu and the multiplayer menu")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    protected virtual void OnEnable()
    {
        EventManager.OnGameOver += DisplayGameOverUI;
    }

    protected virtual void OnDisable()
    {
        EventManager.OnGameOver -= DisplayGameOverUI;
    }

    protected virtual void OnDestroy()
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
    public void DisplayGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    /// <summary>
    /// Method <c>ToggleMainMenuMultiplayerUI</c> Toggles between the Default Menu UI and the Multiplayer Menu UI in the Main Menu Screen
    /// </summary>
    public void ToggleMainMenuMultiplayerUI()
    {
        bool multiplayerUIIsToggled = multiplayerMenuUI.activeSelf;
        multiplayerMenuUI.SetActive(!multiplayerUIIsToggled);
        defaultMenuUI.SetActive(multiplayerUIIsToggled);

    }

    /// <summary>
    /// Method <c>ToggleMainMenuMultiplayerProgressUI</c> Toggles the connection progress label
    /// </summary>
    public virtual void ToggleMainMenuMultiplayerProgressUI()
    {
        bool progressLabelIsToggled = progressLabel.activeSelf;
        progressLabel.SetActive(!progressLabelIsToggled);
        controlPanel.SetActive(progressLabelIsToggled);
    }
}
