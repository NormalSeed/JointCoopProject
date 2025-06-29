using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherState : BaseState
{
    protected SkeletonArcherController _controller;

    public SkeletonArcherState(SkeletonArcherController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}

public class SkeletonArcher_Attack1 : SkeletonArcherState
{
    public SkeletonArcher_Attack1(SkeletonArcherController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Attack1 애니메이션 재생
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
        // State에서는 공격 방향만 결정하고 Attack1 기능 실행(ShootArrow)은 Animation Event로
        _controller.GetAttack1Dir();
    }

    public override void Update()
    {
        base.Update();
        if (_controller._movement._isTrace && !_controller._isAttack1)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
        else if (!_controller._isAttack1 && _controller._isDamaged)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Damaged]);
        }
    }
}
