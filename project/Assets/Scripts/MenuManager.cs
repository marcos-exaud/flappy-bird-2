using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadSingleplayer()
    {
        SceneManager.LoadScene(Consts.BASE_GAME_SCENE);
    }
}
