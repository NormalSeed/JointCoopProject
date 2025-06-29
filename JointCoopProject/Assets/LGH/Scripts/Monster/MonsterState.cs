using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterState : BaseState
{
    protected MonsterBase _controller;

    public MonsterState(MonsterBase controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        if (_controller._isAttack1 && !_controller._isDamaged && _controller._isActivated)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Attack1]);
        }

        if (_controller._isDamaged)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Damaged]);
        }

        if (_controller._isDead)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Dead]);
        }
    }

    public override void Exit()
    {

    }
}

public class Monster_Idle : MonsterState
{
    public Monster_Idle(MonsterBase controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // TODO: view에서 Idle 애니메이션 재생
        _controller._view.PlayAnimation(_controller.IDLE_HASH);
    }

    public override void Update()
    {
        base.Update();
        if (_controller._movement._isPatrol)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Patrol]);
        }

        if (_controller._movement._isTrace)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }
}

public class Monster_Patrol : MonsterState
{
    public Monster_Patrol(MonsterBase controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter()
    {
        // TODO: view에서 Walk 애니메이션 재생
        _controller._view.PlayAnimation(_controller.MOVE_HASH);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        _controller._movement.Patrol(_controller._model._moveSpd);
    }
}

public class Monster_Trace : MonsterState
{
    public Monster_Trace(MonsterBase controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter()
    {
        // TODO : view에서 Move 애니메이션 재생
        _controller._view.PlayAnimation(_controller.MOVE_HASH);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        _controller._movement.Trace(_controller._model._moveSpd);
    }
}

public class Monster_Damaged : MonsterState
{
    public Monster_Damaged(MonsterBase controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _controller._view.PlayAnimation(_controller.DAMAGED_HASH);
    }

    public override void Update()
    {
        base.Update();
        if (!_controller._isDamaged)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Idle]);
        }
    }
}

public class Monster_Dead : MonsterState
{
    public Monster_Dead(MonsterBase controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _controller._view.PlayAnimation(_controller.DEAD_HASH);
    }

    public override void Update()
    {
        
    }
}