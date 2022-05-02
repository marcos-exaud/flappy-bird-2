using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WGameObject
{
    [SerializeField]
    private GameObject gameObject;

    public GameObject GameObject { get { return gameObject; } }

    public WGameObject() { }

    public WGameObject(GameObject gameObject)
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
