using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public void LoadSingleplayer()
    {
        SceneManager.LoadScene(Consts.BASE_GAME_SCENE);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(Consts.STARTING_SCENE);
    }

    public virtual void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
