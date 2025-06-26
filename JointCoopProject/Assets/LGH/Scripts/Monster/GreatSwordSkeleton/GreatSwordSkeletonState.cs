using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordSkeletonState : BaseState
{
    protected GreatSwordSkeletonController _controller;

    public GreatSwordSkeletonState(GreatSwordSkeletonController controller)
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

public class GreatSwordSkeleton_Attack1 : GreatSwordSkeletonState
{
    public GreatSwordSkeleton_Attack1(GreatSwordSkeletonController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Attack1 애니메이션 재생
        int random = Random.Range(0, 2);
        
        
        // Attack1 기능 실행
        _controller.Attack1();
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
