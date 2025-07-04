using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class WarriorState : BaseState
{
    protected WarriorController _controller;

    public WarriorState(WarriorController controller)
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

public class Warrior_Attack1 : WarriorState
{
    public Warrior_Attack1(WarriorController controller) : base(controller)
    {
        _hasPhysics = false;
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
        _controller._shieldDuration -= Time.deltaTime;
        if (_controller._shieldDuration <= 0)
        {
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Idle]);
        }
        if (_controller._curShield - _controller._model._curHP.Value >= 300)
        {
            _controller._model._curHP.Value = _controller._curShield - 300;
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Stun]);
        }
    }

    public override void Exit()
    {
        SoundManager.Instance.RStopSFX();
        _controller._shieldDuration = 5f;
        _controller._curShield = 0;
        _controller.EndAttack1();
        _controller._attack1Cooldown = 15;
    }
}

public class Warrior_Attack2 : WarriorState
{
    public Warrior_Attack2(WarriorController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Attack2 애니메이션 재생
        _controller._view.PlayAnimation(_controller.ATTACK2_HASH);
        // Attack2 기능 실행(GetAttack2Dir), Attack2()는 애니메이션 이벤트로 할당
        _controller.GetAttack2Dir();
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
        _controller._attack2Cooldown = 3f;
    }
}

public class Warrior_Stun : WarriorState
{
    private float _stunDuration = 5f;
    public Warrior_Stun(WarriorController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Stun 애니메이션 재생
        _controller._view.PlayAnimation(_controller.STUN_HASH);
    }

    public override void Update()
    {
        _stunDuration -= Time.deltaTime;
        if (_stunDuration < 0)
        {
            _controller._movement._isTrace = true;
            _controller._stateMachine.ChangeState(_controller._stateMachine._stateDic[EState.Trace]);
        }
    }

    public override void Exit()
    {
        _stunDuration = 5f;
    }
}
