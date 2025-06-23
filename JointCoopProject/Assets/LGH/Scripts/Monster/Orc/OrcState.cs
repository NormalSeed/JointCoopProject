using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcState : BaseState
{
    protected OrcController _controller;

    public OrcState(OrcController controller)
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

public class Orc_Attack1 : OrcState
{
    public Orc_Attack1(OrcController controller) : base(controller)
    {
        _hasPhysics = false;
    }

    public override void Enter()
    {
        // Attack1 애니메이션 재생
    }

    public override void Update()
    {
        base.Update();

    }
}
