using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rigidbody2DWrapper
{
    public Rigidbody2D rigidbody2D;

    public Rigidbody2DWrapper()
    {
        rigidbody2D = new Rigidbody2D();
    }

    public Rigidbody2DWrapper(Rigidbody2D rigidbody2D)
    {
        this.rigidbody2D = rigidbody2D;
    }

    public virtual Vector2 velocity
    {
        get { return rigidbody2D.velocity; }
        set { rigidbody2D.velocity = value; }
    }

    public virtual Vector2 position
    {
        get { return rigidbody2D.position; }
        set { rigidbody2D.position = value; }
    }

    public virtual bool IsAwake()
    {
        return rigidbody2D.IsAwake();
    }

    public virtual bool IsSleeping()
    {
        return rigidbody2D.IsSleeping();
    }

    public virtual void Sleep()
    {
        rigidbody2D.Sleep();
    }

    public virtual void WakeUp()
    {
        rigidbody2D.WakeUp();
    }
}
