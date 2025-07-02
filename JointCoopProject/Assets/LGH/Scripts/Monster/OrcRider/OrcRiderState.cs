using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcRiderState : BaseState
{
    protected OrcRiderController _controller;

    public OrcRiderState(OrcRiderController controller)
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

public class OrcRider_Attack1 : OrcRiderState
{
    private float _rushDelay = 0.5f;

    public OrcRider_Attack1(OrcRiderController controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter()
    {
        // Attack1 애니메이션 재생
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
        // Attack1 기능 실행(Attack1)
        _controller.Attack1();
    }

    public override void Update()
    {
        if (_rushDelay > 0f) _rushDelay -= Time.deltaTime;

        if (_controller._movement._isTrace && !_controller._isAttack1)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Stun]);
        }
    }

    public override void FixedUpdate()
    {
        if (_rushDelay <= 0f)
        {
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination, _controller._model._moveSpd * 1.5f * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        _rushDelay = 0.5f;
    }
}

public class OrcRider_Attack2 : OrcRiderState
{
    public OrcRider_Attack2(OrcRiderController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Attack2 애니메이션 재생
        _controller._view.PlayAnimation(_controller.ATTACK2_HASH);
        // Attack2 기능 실행(ShootBullet)
        _controller.GetAttack2Dir();
    }

    public override void Update()
    {
        base.Update();
        if (_controller._movement._isTrace && !_controller._isAttack2)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }
}

public class OrcRider_Stun : OrcRiderState
{
    private float stunDuration = 4f;

    public OrcRider_Stun(OrcRiderController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _controller._view.PlayAnimation(_controller.IDLE_HASH);
    }

    public override void Update()
    {
        base.Update();
        stunDuration -= Time.deltaTime;

        if (stunDuration <= 0)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }

    public override void Exit()
    {
        base.Exit();
        stunDuration = 4f;
    }
}
