using Photon.Pun;

public class MultiplayerMenuManager : MenuManager
{
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
