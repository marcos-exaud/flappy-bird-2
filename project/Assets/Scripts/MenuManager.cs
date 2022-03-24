using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuManager : MonoBehaviour
{
    public void LoadSingleplayer()
    {
        SceneManager.LoadScene(Consts.BASE_GAME_SCENE);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Consts.STARTING_SCENE);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomOrCreateRoom(null , 2);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}
