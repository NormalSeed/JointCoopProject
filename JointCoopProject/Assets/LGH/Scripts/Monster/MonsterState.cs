using System.Collections;
using System.Collections.Generic;
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
    }

    public override void Update()
    {
        base.Update();
        if (_controller._movement._canMove)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Patrol]);
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
        // TODO: view에서 Patrol 애니메이션 재생
    }

    public override void Update()
    {
        base.Update();
        if (!_controller._movement._canMove)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Idle]);
        }
    }

    public override void FixedUpdate()
    {
        _controller._movement.Patrol();
    }
}