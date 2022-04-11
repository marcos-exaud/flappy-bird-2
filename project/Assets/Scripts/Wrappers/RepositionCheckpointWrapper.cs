using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionCheckpointWrapper : MonoBehaviour
{
    public RepositionCheckpoint repositionCheckpoint;

    public RepositionCheckpointWrapper()
    {
        repositionCheckpoint = new RepositionCheckpoint();
    }

    public RepositionCheckpointWrapper(RepositionCheckpoint repositionCheckpoint)
    {
        this.repositionCheckpoint = repositionCheckpoint;
    }
    
    public virtual float GetLastObstacleHeight()
    {
        return repositionCheckpoint.GetLastObstacleHeight();
    }

    public virtual void SetLastObstacleHeight(float value)
    {
        repositionCheckpoint.SetLastObstacleHeight(value);
    }
}
