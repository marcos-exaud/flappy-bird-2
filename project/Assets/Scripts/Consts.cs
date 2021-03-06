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
    public const int STARTING_SCENE = 0;
    public const int BASE_GAME_SCENE = 1;
    public const int MULTIPLAYER_GAME_SCENE_1P = 2;
    public const int MULTIPLAYER_GAME_SCENE_2P = 3;

    // game's horizontal scrolling speed
    public static readonly float GAME_X_SCROLLING_SPEED = 5f;
}
