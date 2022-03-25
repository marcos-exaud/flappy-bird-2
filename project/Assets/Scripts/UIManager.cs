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
}
