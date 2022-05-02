using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public WGameObject gameObject { get; }

    public bool GetReady();
    public void SetReady(bool value);

    public void ChangeSprite();

    public void TogglePhysics();
}
