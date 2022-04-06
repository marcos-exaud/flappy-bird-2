using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectWrapper
{
    private GameObject gameObject;

    public GameObjectWrapper()
    {
        this.gameObject = new GameObject();
    }

    public GameObjectWrapper(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public virtual void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual T GetComponent<T>()
    {
        return gameObject.GetComponent<T>();
    }
}
