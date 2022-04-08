using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWrapper
{
    public virtual bool GetKeyDown(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}
