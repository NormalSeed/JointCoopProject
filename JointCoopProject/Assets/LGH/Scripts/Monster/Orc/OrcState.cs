using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcState : BaseState
{
    protected OrcController _controller;

    public OrcState(OrcController controller)
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

public class Orc_Attack1 : OrcState
{
    public Orc_Attack1(OrcController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Attack1 애니메이션 재생
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
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
