using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

public class Warrior_Attack2 : WarriorState
{
    public Warrior_Attack2(WarriorController controller) : base(controller)
    {
        _hasPhysics = false;
    }
}
