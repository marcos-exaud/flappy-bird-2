using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static Action<GameObject> OnObstacleClear;

    public const byte OnGameOverPhotonEventCode = 2;
    public static Action OnGameOver;

    public const byte OnPlayerDeathPhotonEventCode = 3;
    public static Action OnPlayerDeath;

    public static Action OnPlayerReadyUp;
}
