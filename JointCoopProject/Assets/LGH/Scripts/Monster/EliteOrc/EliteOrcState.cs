using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteOrcState : BaseState
{
    protected EliteOrcController _controller;

    public EliteOrcState(EliteOrcController controller)
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

public class EliteOrc_Attack1 : EliteOrcState
{
    private float _rushDelay = 0.5f;
    public EliteOrc_Attack1(EliteOrcController controller) : base(controller)
    {
        _hasPhysics = true;
    }

    public override void Enter()
    {
        // Attack1 애니메이션 재생과 기능이 Attack1에 통합
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
            _controller.gameObject.transform.position = Vector2.MoveTowards(_controller.gameObject.transform.position, _controller._rushDestination, _controller._model._moveSpd * 2f * Time.deltaTime);
        }
    }

    public override void Exit()
    {
        _rushDelay = 0.5f;
    }
}

public class EliteOrc_Stun : EliteOrcState
{
    private float stunDuration = 4f;
    public EliteOrc_Stun(EliteOrcController controller) : base(controller)
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
        stunDuration = 4f;
    }
}
