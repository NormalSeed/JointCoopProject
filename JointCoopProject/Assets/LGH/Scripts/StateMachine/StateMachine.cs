using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Dictionary<EState, BaseState> _stateDic;
    public BaseState _curState;
    public StateMachine()
    {
        _stateDic = new Dictionary<EState, BaseState>();
    }

    public void ChangeState(BaseState changedState)
    {
        if (_curState == changedState) return;

        _curState.Exit();
        _curState = changedState;
        _curState.Enter();
    }

    public void Update() => _curState.Update();

    public void FixedUpdate()
    {
        if (_curState._hasPhysics) _curState.FixedUpdate();
    }
}
