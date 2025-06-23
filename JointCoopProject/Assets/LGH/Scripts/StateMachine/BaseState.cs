using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public bool HasPhysics;

    public abstract void Enter();
    public abstract void Update();
    public virtual void FixedUpdate() { }
    public abstract void Exit();
}

public enum EState
{
    Idle, Run, Jump, Patrol, CantControll, Attack, SpAttack1, SpAttack2, Detect, Terrorized
}
