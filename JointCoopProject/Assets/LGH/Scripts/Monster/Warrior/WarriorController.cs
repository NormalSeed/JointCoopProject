using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorController : MonsterBase
{
    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");
    public readonly int ATTACK2_HASH = Animator.StringToHash("Attack2");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10252;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Warrior_Attack1(this));
        _stateMachine._stateDic.Add(EState.Attack1, new Warrior_Attack2(this));
    }

    // Attack1: 실드 패턴, 5초간 지속되고 체력 300만큼의 실드를 획득하고 제자리에 멈춰 있음
    // 지속시간이 끝나거나 실드가 모두 소모되면 패턴이 종료되고 종료된 시점부터 15초의 쿨다운을 갖는다.

    // Attack2: 부채꼴 공격 패턴, 공격받은 Player는 기절해서 잠시 못움직임

    
}
