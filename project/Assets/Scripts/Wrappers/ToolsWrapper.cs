using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsWrapper
{
    public ToolsWrapper() { }

    public virtual float LimitedRandomVariance(float value, float min, float max, float maxAbsVariance)
    {
        return Tools.LimitedRandomVariance(value, min, max, maxAbsVariance);
    }
}
