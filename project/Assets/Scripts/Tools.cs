using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    // receives a value, and randomly changes it into a different value within given bounds
    public static float LimitedRandomVariance(float value, float min, float max, float maxAbsVariance)
    {
        return Random.Range(Mathf.Max(min, value - maxAbsVariance), Mathf.Min(max, value + maxAbsVariance));
    }
}
