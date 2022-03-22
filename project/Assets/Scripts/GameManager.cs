using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private GameObject[] obstacles;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        // wait for player to input first movement to start the game
        yield return WaitForInput(KeyCode.UpArrow);

        // wake up all players and obstacles to properly start the game
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerManager>().WakeUp();
        }
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.GetComponent<ObstacleManager>().WakeUp();
        }
    }

    private IEnumerator WaitForInput(KeyCode key)
    {
        while (true)
        {
            // if user input wasnt received, restart loop
            if (!Input.GetKeyDown(key))
            {
                // waits one frame
                yield return null;
                continue;
            }

            //if user input was received, this line will be reached and it will end the loop
            break;
        }
    }
}
