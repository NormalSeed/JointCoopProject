using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedGoblinController : MonsterBase
{
    private InfectedGoblinFireController _fireController;
    private float _attack1Cooldown = 4f;
    private Vector2 _attack1Dir1;
    private Vector2 _attack1Dir2;
    private Vector2 _attack1Dir3;


    protected override void Init()
    {
        base.Init();
        _monsterID = 10302;
        _fireController = GetComponentInChildren<InfectedGoblinFireController>();
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
            GetAttack1Dir();
            ShootFire();
            _attack1Cooldown = 4f;
        }
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
    }

    private void GetAttack1Dir()
    {
        _attack1Dir1 = (_player.transform.position - transform.position).normalized;
        _attack1Dir2 = (_player.transform.position - transform.position).normalized;
        float angle = 15f;
        
        float rad2 = angle * Mathf.Deg2Rad;
        float rad3 = -angle * Mathf.Deg2Rad;

        float rotated2X = _attack1Dir2.x * Mathf.Cos(rad2) - _attack1Dir2.y * Mathf.Sin(rad2);
        float rotated2Y = _attack1Dir2.x * Mathf.Sin(rad2) + _attack1Dir2.y * Mathf.Cos(rad2);

        float rotated3X = _attack1Dir2.x * Mathf.Cos(rad3) - _attack1Dir2.y * Mathf.Sin(rad3);
        float rotated3Y = _attack1Dir2.x * Mathf.Sin(rad3) + _attack1Dir2.y * Mathf.Cos(rad3);

        _attack1Dir2 = new Vector2(rotated2X, rotated2Y);
        _attack1Dir3 = new Vector2(rotated3X, rotated3Y);
    }

    public void ShootFire()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_InfectedGoblinAttack);
        _fireController.ShootFire(_attack1Dir1, _model._attack1Damage);
        _fireController.ShootFire(_attack1Dir2, _model._attack1Damage);
        _fireController.ShootFire(_attack1Dir3, _model._attack1Damage);
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_InfectedGoblinDie);
    }
}
