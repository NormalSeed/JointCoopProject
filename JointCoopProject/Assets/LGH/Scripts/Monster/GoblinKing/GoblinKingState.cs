using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingState : BaseState
{
    protected GoblinKingController _controller;

    public GoblinKingState(GoblinKingController controller)
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

public class GoblinKing_Attack1 : GoblinKingState
{
    public GoblinKing_Attack1(GoblinKingController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
        _controller.Attack1();
    }

    public override void Update()
    {
        if (_controller._isAttack2 && !_controller._isAttack1)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Attack2]);
        }
        if (_controller._isDead)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Dead]);
        }
    }
}

public class GoblinKing_Attack2 : GoblinKingState
{
    public GoblinKing_Attack2(GoblinKingController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _controller._view.PlayAnimation(_controller.ATTACK2_HASH);
    }

    public override void Update()
    {
        if (_controller._isAttack1 && !_controller._isAttack2)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Attack1]);
        }
        if (_controller._isDead)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Dead]);
        }
    }
}