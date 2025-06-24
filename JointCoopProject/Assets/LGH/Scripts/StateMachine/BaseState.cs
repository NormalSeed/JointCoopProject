using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public bool _hasPhysics;

    public abstract void Enter();
    public abstract void Update();
    public virtual void FixedUpdate() { }
    public abstract void Exit();
}

public enum EState
{
    Idle, Patrol, Trace, Attack1, Attack2
}
