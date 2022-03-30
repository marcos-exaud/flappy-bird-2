using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerObstacleManager : ObstacleManager
{
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

            float newHeight = Tools.LimitedRandomVariance(repositionCheckpoint.GetLastObstacleHeight(),
                                                            Consts.MIN_GAP_HEIGHT,
                                                            Consts.MAX_GAP_HEIGHT,
                                                            Consts.MAX_ABS_VARIANCE);
                                                            
            photonView.RPC("RepositionY", RpcTarget.All, newHeight);

            repositionCheckpoint.SetLastObstacleHeight(newHeight);
        }
        else if (intruder.layer == LayerMask.NameToLayer("Player") && intruder.GetComponent<PhotonView>().IsMine) // increment the score
        {
            EventManager.OnObstacleClear?.Invoke(intruder);
        }
    }

    // repositions the obstacle in the y axis
    [PunRPC]
    public override void RepositionY(float value)
    {
        obstacle.position = new Vector2(obstacle.position.x, value);
    }
}
