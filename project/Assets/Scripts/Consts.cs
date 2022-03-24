using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    // limit values for the center of the gap in an obstacle
    public static readonly float MAX_GAP_HEIGHT = 3f;
    public static readonly float MIN_GAP_HEIGHT = -3f;

    // maximum absolute height change between subsequent obstacles
    public static readonly float MAX_ABS_VARIANCE = 3f;

    // scene values
    public static readonly int STARTING_SCENE = 0;
    public static readonly int BASE_GAME_SCENE = 1;
}
