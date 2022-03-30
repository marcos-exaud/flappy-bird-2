using UnityEngine;
using UnityEngine.SceneManagement;

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

    public virtual void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
