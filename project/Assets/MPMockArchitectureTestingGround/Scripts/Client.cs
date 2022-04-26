using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public sealed class Client
{
    private static readonly Client instance = new Client();
    
    static Client() { }

    private Client() { }

    public static Client Instance
    {
        get
        {
            return instance;
        }
    }
}
