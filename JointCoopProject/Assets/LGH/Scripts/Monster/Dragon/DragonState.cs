using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonState : BaseState
{
    protected DragonController _controller;

    public DragonState(DragonController controller)
    {
        _controller = controller;
    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        if (_controller._isDead)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Dead]);
        }
    }

    public override void Exit()
    {
        
    }
}

public class Dragon_Attack1 : DragonState
{
    public Dragon_Attack1(DragonController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Attack1 애니메이션 재생
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
        // Attack1 기능 실행(ShootFlame)
        _controller.GetAttack1Dir1();
        _controller.GetAttack1Dir2();
        _controller.ShootFlame();
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
