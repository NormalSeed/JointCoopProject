using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonState : BaseState
{
    protected SkeletonController _controller;

    public SkeletonState(SkeletonController controller)
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

public class Skeleton_Attack1 : SkeletonState
{
    public Skeleton_Attack1(SkeletonController controller) : base(controller)
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
        if (_controller._movement._isTrace && !_controller._isAttack1)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
        
        if (!_controller._isAttack1 && _controller._isDamaged)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Damaged]);
        }
    }
}
