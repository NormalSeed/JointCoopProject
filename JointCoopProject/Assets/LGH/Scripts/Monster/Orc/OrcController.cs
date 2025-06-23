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


}
