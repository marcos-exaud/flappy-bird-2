using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class MultiplayerObstacleManager : ObstacleManager
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
                FallAsleep();
                break;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D intruderCollider)
    {
        GameObject intruder = intruderCollider.gameObject;

        // act depending on the other collider:
        if (intruder.layer == LayerMask.NameToLayer("Reset Checkpoint")) // loop the obstacle to the other side of the game
        {
            Cycle();
        }
        else if (intruder.layer == LayerMask.NameToLayer("Reposition Checkpoint") && PhotonNetwork.IsMasterClient) // change the obstacle's height
        {
            RepositionCheckpoint repositionCheckpoint = intruder.GetComponent<RepositionCheckpoint>();

            float newHeight = tools.LimitedRandomVariance(repositionCheckpoint.GetLastObstacleHeight(),
                                                            Consts.MIN_GAP_HEIGHT,
                                                            Consts.MAX_GAP_HEIGHT,
                                                            Consts.MAX_ABS_VARIANCE);

            photonView.RPC("RepositionY", RpcTarget.All, newHeight);

            repositionCheckpoint.SetLastObstacleHeight(newHeight);
        }
        else if (intruder.layer == LayerMask.NameToLayer("Player") && intruder.GetComponent<PhotonView>().IsMine) // increment the score
        {
            EventManager.OnObstacleClear?.Invoke(new GameObjectWrapper(intruder));
        }
    }

    // repositions the obstacle in the y axis
    [PunRPC]
    public override void RepositionY(float value)
    {
        obstacle.position = new Vector2(obstacle.position.x, value);
    }

    public void FallAsleep()
    {
        obstacle.Sleep();
        obstacle.velocity = new Vector2(0f, 0f);
    }
}
