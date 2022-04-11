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

    // Wrappers
    private GameObjectWrapper gameOverUIWrapper;
    private GameObjectWrapper multiplayerMenuUIWrapper;
    private GameObjectWrapper defaultMenuUIWrapper;
    private GameObjectWrapper controlPanelWrapper;
    private GameObjectWrapper progressLabelWrapper;

    protected virtual void Start()
    {
        InitWrappers();
    }

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

    private void InitWrappers()
    {
        gameOverUIWrapper = new GameObjectWrapper(gameOverUI);
        multiplayerMenuUIWrapper = new GameObjectWrapper(multiplayerMenuUI);
        defaultMenuUIWrapper = new GameObjectWrapper(defaultMenuUI);
        controlPanelWrapper = new GameObjectWrapper(controlPanel);
        progressLabelWrapper = new GameObjectWrapper(progressLabel);
    }

    public virtual void UpdateScoreboard(GameObjectWrapper player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        int clearerScore = playerManager.GetPlayerScore();

        scoreboard.text = "" + clearerScore;
    }

    // display game over screen
    public void DisplayGameOverUI()
    {
        gameOverUIWrapper.SetActive(true);
    }

    /// <summary>
    /// Method <c>ToggleMainMenuMultiplayerUI</c> Toggles between the Default Menu UI and the Multiplayer Menu UI in the Main Menu Screen
    /// </summary>
    public void ToggleMainMenuMultiplayerUI()
    {
        bool multiplayerUIIsToggled = multiplayerMenuUIWrapper.activeSelf;
        multiplayerMenuUIWrapper.SetActive(!multiplayerUIIsToggled);
        defaultMenuUIWrapper.SetActive(multiplayerUIIsToggled);

    }

    /// <summary>
    /// Method <c>ToggleMainMenuMultiplayerProgressUI</c> Toggles the connection progress label
    /// </summary>
    public virtual void ToggleMainMenuMultiplayerProgressUI()
    {
        bool progressLabelIsToggled = progressLabelWrapper.activeSelf;
        progressLabelWrapper.SetActive(!progressLabelIsToggled);
        controlPanelWrapper.SetActive(progressLabelIsToggled);
    }
}
