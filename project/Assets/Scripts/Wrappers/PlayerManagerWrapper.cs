using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerWrapper
{
    public PlayerManager playerManager;

    public PlayerManagerWrapper()
    {
        playerManager = new PlayerManager();
    }

    public PlayerManagerWrapper(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }

    public virtual void Kill()
    {
        playerManager.Kill();
    }
}
