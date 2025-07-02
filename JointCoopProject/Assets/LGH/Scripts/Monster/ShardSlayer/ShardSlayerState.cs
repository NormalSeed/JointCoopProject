using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardSlayerState : BaseState

{
    protected ShardSlayerController _controller;

    public ShardSlayerState(ShardSlayerController controller)
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

public class ShardSlayer_Attack1 : ShardSlayerState
{
    private float _firstRushDuration;
    public ShardSlayer_Attack1(ShardSlayerController controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter()
    {
        _firstRushDuration = 1f;
        _controller._view.PlayAnimation(_controller.ATTACK1_HASH);
        _controller.Attack1();
    }

    public override void Update()
    {
        _firstRushDuration -= Time.deltaTime;
        if (_controller._movement._isTrace && !_controller._isAttack1)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }

    public override void FixedUpdate()
    {
        if (_firstRushDuration > 0f)
        {
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination1, _controller._model._attack1Range * Time.deltaTime);
        }
        else
        {
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination2, _controller._model._attack1Range * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        _controller._attackType = 2;
    }
}

public class ShardSlayer_Attack2 : ShardSlayerState
{
    public ShardSlayer_Attack2(ShardSlayerController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        _controller._view.PlayAnimation(_controller.ATTACK2_HASH);
        _controller.Attack2();
    }

    public override void Update()
    {
        if (_controller._movement._isTrace && !_controller._isAttack2)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }

    public override void Exit()
    {
        _controller._attackType = 1;
    }
}
