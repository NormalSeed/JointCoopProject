using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredOrcState : BaseState
{
    protected ArmoredOrcController _controller;

    public ArmoredOrcState(ArmoredOrcController controller)
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

public class ArmoredOrc_Attack1 : ArmoredOrcState
{
    private float _rushDelay;
    public ArmoredOrc_Attack1(ArmoredOrcController controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter()
    {
        _rushDelay = 0.5f;
        // Attack1 애니메이션 재생
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
        // Attack1 기능 실행
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
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination, _controller._model._attack1Range * 2 * Time.deltaTime);
        }
    }
}

public class ArmoredOrc_Stun : ArmoredOrcState
{
    private float stunDuration = 3f;
    public ArmoredOrc_Stun(ArmoredOrcController controller) : base(controller)
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
        stunDuration = 3f;
    }
}