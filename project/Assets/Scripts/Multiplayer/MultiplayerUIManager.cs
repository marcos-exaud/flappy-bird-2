using System.Collections;
using UnityEngine;
using TMPro;
using Photon.Pun;
using ExitGames.Client.Photon;

public class MultiplayerUIManager : UIManager
{
    [Tooltip("The Ui Panel containing the rematch menu")]
    [SerializeField]
    private GameObject rematchUI;

    [SerializeField]
    private TextMeshProUGUI readyUpLabel;
    [SerializeField]
    private TextMeshProUGUI readyUpCountdown;

    [SerializeField]
    private string readyUpPrompt = "Press UP Arrow to Ready Up!";
    [SerializeField]
    private string playerReadyLabel = "You are ready! Waiting for other players...";

    void Start()
    {
        readyUpLabel.SetText(readyUpPrompt);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        EventManager.OnPlayerReadyUp += TogglePlayerReadyLabel;

        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        EventManager.OnPlayerReadyUp -= TogglePlayerReadyLabel;

        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.OnPlayerReadyUp -= TogglePlayerReadyLabel;

        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    public void OnPhotonEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        switch (eventCode)
        {
            case EventManager.OnGameOverPhotonEventCode:
                DisplayGameOverUI();
                break;
        }
    }

    public void TogglePlayerReadyLabel()
    {
        readyUpLabel.SetText(playerReadyLabel);
    }

    public IEnumerator Countdown()
    {
        readyUpLabel.enabled = false;
        readyUpCountdown.enabled = true;

        for (int i = 3; i > 0; i--)
        {
            readyUpCountdown.SetText("-- " + i + " --");
            yield return new WaitForSeconds(1f);
        }

        StartCoroutine("CountdownEnded");
    }

    private IEnumerator CountdownEnded()
    {
        readyUpCountdown.SetText("GO!");
        yield return new WaitForSeconds(2f);
        readyUpCountdown.enabled = false;
    }

    public void ToggleRematchUI()
    {
        gameOverUI.SetActive(false);
        rematchUI.SetActive(true);
    }
}
