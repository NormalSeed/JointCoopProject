using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedGhostController : MonsterBase
{
    private InfectedGhostFireController _fireController;
    private float _attack1Cooldown = 3f;

    protected override void Init()
    {
        base.Init();
        _monsterID = 10301;

        _fireController = GetComponentInChildren<InfectedGhostFireController>();
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
    }

    protected override void Update()
    {
        base.Update();
        if (_attack1Cooldown > 0)
        {
            _attack1Cooldown -= Time.deltaTime;
        }
        else
        {
            ShootFire();
            _attack1Cooldown = 3f;
        }
    }

    public void ShootFire()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_InfectedGhostAttack);
        _fireController.ShootFire(Vector2.up, _model._attack1Damage);
        _fireController.ShootFire(Vector2.down, _model._attack1Damage);
        _fireController.ShootFire(Vector2.right, _model._attack1Damage);
        _fireController.ShootFire(Vector2.left, _model._attack1Damage);
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_InfectedGhostDie);
    }
}
