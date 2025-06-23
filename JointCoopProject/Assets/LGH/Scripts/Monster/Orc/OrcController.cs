using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonsterBase
{
    private bool isAttacking;

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Orc_Attack1(this));
    }

    private void Attack1()
    {
        // 전방으로 도끼를 휘두른 후 1초 멈춤
        // 1초 멈춰야 하므로 Coroutine 사용 필요
    }
}
