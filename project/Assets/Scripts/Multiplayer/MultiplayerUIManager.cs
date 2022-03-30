using Photon.Pun;
using ExitGames.Client.Photon;

public class MultiplayerUIManager : UIManager
{
    protected override void OnEnable()
    {
        base.OnEnable();

        PhotonNetwork.NetworkingClient.EventReceived += OnPhotonEvent;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        PhotonNetwork.NetworkingClient.EventReceived -= OnPhotonEvent;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

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
}
