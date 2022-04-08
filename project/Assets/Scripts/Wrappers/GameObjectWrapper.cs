using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectWrapper
{
    public GameObject gameObject;

    public GameObjectWrapper()
    {
        this.gameObject = new GameObject();
    }

    public GameObjectWrapper(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public virtual bool activeSelf
    {
        get { return gameObject.activeSelf; }
    }

    public virtual void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public virtual T GetComponent<T>()
    {
        return gameObject.GetComponent<T>();
    }

    public virtual T[] GetComponentsInChildren<T>()
    {
        return gameObject.GetComponentsInChildren<T>();
    }
}
