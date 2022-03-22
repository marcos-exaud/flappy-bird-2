using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // player rigidbody
    private Rigidbody2D bird;

    void Start()
    {
        bird = GetComponent<Rigidbody2D>();
    }

    public void Kill()
    {
        // to do
        Debug.Log("dies");
        SceneManager.LoadScene("SampleScene");
    }

    public void WakeUp()
    {
        if (bird.IsSleeping())
        {
            bird.WakeUp();
        }
    }
}
