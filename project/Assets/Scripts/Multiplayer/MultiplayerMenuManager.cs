using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerMenuManager : MenuManager, IInRoomCallbacks
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void Reload()
    {
        Hashtable hash = new Hashtable();
        hash.Add("Rematch", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("Rematch");
        }
    }

    private IEnumerator Rematch()
    {
        yield return new WaitUntil(() => PlayersAreReady());

        if (PhotonNetwork.IsConnected)
        {
            MultiplayerGameManager.DestroyAllPlayerGameObjects();
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel(MultiplayerGameManager.GetNextMultiplayerScene());
        }
    }

    private bool PlayersAreReady()
    {
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            bool ready = (bool)player.CustomProperties["Rematch"];
            if (!ready) return false;
        }
        return true;
    }
}
