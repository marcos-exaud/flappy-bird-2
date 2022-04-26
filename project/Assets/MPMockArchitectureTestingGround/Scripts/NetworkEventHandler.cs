using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class NetworkEventHandler
{
    // Delegates
    public delegate void RemotePlayerInstanceAction(int playerNetID);

    // Events
    public static event RemotePlayerInstanceAction onRemotePlayerReadyUp;

    public enum NetworkEventCodes
    {
        onRemotePlayerReadyUp = (byte)61
    }

    private static NetworkEventCodes nec;

    private static Dictionary<byte, Action> dic;

    public static void ProcessEvent(byte code, object[] data)
    {
        switch (code)
        {
            case (byte)NetworkEventCodes.onRemotePlayerReadyUp:
                onRemotePlayerReadyUp?.Invoke((int)data[0]);
                break;
            default:
                break;
        }
    }
}
