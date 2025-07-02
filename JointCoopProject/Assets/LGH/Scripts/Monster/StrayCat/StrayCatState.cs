using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrayCatState : BaseState
{
    protected StrayCatController _controller;

    public StrayCatState(StrayCatController controller)
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

public class StrayCat_Attack1 : StrayCatState
{
    public StrayCat_Attack1(StrayCatController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        Debug.Log("Attack1 진입");
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
        _controller.Attack1();
    }

    public override void Update()
    {
        if (!_controller._isAttack1 && _controller._movement._isTrace)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }

    public override void Exit()
    {
        _controller._typeChangeDelay = 4f;
    }
}

public class StrayCat_Attack2 : StrayCatState
{
    public StrayCat_Attack2(StrayCatController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        Debug.Log("Attack2 진입");
        _controller._view.PlayAnimation(_controller.ATTACK2_HASH);
        _controller.SetAttack2();
    }

    public override void Update()
    {
        if (!_controller._isAttack2 && _controller._movement._isTrace)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }

    public override void Exit()
    {
        _controller._typeChangeDelay = 4f;
    }
}

public class StrayCat_Attack3 : StrayCatState
{
    private float _firstRushDuration;

    public StrayCat_Attack3(StrayCatController controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter() 
    {
        _firstRushDuration = 1f;
        _controller._view.PlayAnimation(_controller.ATTACK3_HASH);
        _controller.Attack3();
    }

    public override void Update()
    {
        _firstRushDuration -= Time.deltaTime;
        if (_controller._movement._isTrace && !_controller._isAttack3)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }

    public override void FixedUpdate()
    {
        if (_firstRushDuration > 0f)
        {
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination1, _controller._model._attack3Range * Time.deltaTime);
        }
        else
        {
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination2, _controller._model._attack3Range * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        _controller._typeChangeDelay = 4f;
    }
}
