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

        if (_controller._isAttack2 && !_controller._isDamaged && _controller._isActivated)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Attack2]);
        }

        if (_controller._isAttack3 && !_controller._isDamaged && _controller._isActivated)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Attack3]);
        }

        if (_controller._isDamaged && !_controller._isAttack1 && !_controller._isAttack2 && !_controller._isAttack3)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Damaged]);
        }

        if (_controller._isDead)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Dead]);
        }

        if (_controller._isParalyzed)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Paralyze]);
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
    private float _damagedTime;
    public Monster_Damaged(MonsterBase controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _damagedTime = 1f;
        _controller._view.PlayAnimation(_controller.DAMAGED_HASH);
        _controller._movement._rb.velocity = Vector2.zero;
    }

    public override void Update()
    {
        _damagedTime -= Time.deltaTime;

        if (_damagedTime < 0f)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Idle]);
        }

        if (_controller._isParalyzed)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Paralyze]);
        }
    }

    public override void Exit()
    {
        _controller._isDamaged = false;
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

public class Monster_Paralyze : MonsterState
{
    private float _paralyzeTimer;
    public Monster_Paralyze(MonsterBase controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        Debug.Log("스턴 상태 진입");
        _controller._view.PlayAnimation(_controller.IDLE_HASH);
        _paralyzeTimer = 3f;
        _controller._movement._rb.velocity = Vector2.zero;
    }

    public override void Update()
    {
        if (_paralyzeTimer > 0f)
        {
            _paralyzeTimer -= Time.deltaTime;
        }

        if (_paralyzeTimer <= 0f)
        {
            _controller._isParalyzed = false;
        }

        if (!_controller._isParalyzed)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Idle]);
        }

        if (_controller._isDead)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Dead]);
        }
    }
}