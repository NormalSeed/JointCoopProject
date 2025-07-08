using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolluxState : BaseState
{
    protected GolluxController _controller;

    public GolluxState(GolluxController controller)
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

public class Gollux_Attack1 : GolluxState
{
    private float _rushDelay;
    public Gollux_Attack1(GolluxController controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter()
    {
        _controller.Attack1();
        _rushDelay = 0.5f;
    }

    public override void Update()
    {
        base.Update();
        if (_rushDelay > 0f) _rushDelay -= Time.deltaTime;

        if (_controller._movement._isTrace && !_controller._isAttack1)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }

        if (_controller._isDamaged)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Stun]);
        }
    }

    public override void FixedUpdate()
    {
        if (_rushDelay <= 0f)
        {
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination, _controller._model._attack2Range * 1.5f  * Time.deltaTime);
        }
    }
}

public class Gollux_Stun : GolluxState
{
    private float _stunDuration;
    public Gollux_Stun(GolluxController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _controller._view.PlayAnimation(_controller.STUN_HASH);
        _stunDuration = 5f;
    }

    public override void Update()
    {
        base.Update();
        _stunDuration -= Time.deltaTime;

        if (_stunDuration <= 0)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }
}
