using UnityEngine;
using Photon.Pun;

public class MultiplayerMenuManager : MenuManager
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void Reload()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            MultiplayerGameManager.DestroyAllPlayerGameObjects();
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel(MultiplayerGameManager.GetNextMultiplayerScene());
        }
    }
}
