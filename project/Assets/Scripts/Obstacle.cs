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
            GetPlayerManagerWrapperFromGameObject(intruder).Kill();
        }
    }

    protected virtual PlayerManagerWrapper GetPlayerManagerWrapperFromGameObject(GameObject gameObject)
    {
        return new PlayerManagerWrapper(gameObject.GetComponent<PlayerManager>());
    }
}
