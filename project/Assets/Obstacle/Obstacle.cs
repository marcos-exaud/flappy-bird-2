using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D intruderCollider)
    {
        GameObject intruder = intruderCollider.gameObject;

        // if the other collider is a player, kill it
        if (intruder.layer == LayerMask.NameToLayer("Player"))
        {
            intruder.GetComponent<PlayerManager>().Kill();
        }
    }
}
