using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionCheckpoint : MonoBehaviour
{
    // height of the center of the gap of the last checkpoint; default at start is 0
    private float lastObstacleHeight = 0f;

    public float GetLastObstacleHeight()
    {
        return(lastObstacleHeight);
    }
    public void SetLastObstacleHeight(float value)
    {
        lastObstacleHeight = value;
    }
}
