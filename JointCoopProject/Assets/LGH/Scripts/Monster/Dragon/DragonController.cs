using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class DragonController : MonsterBase
{
    [SerializeField] private Transform _mergePoint;
    [SerializeField] private DragonFlameController _flameController;
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(1f);
    public Vector2 _attack1Dir1;
    public Vector2 _attack1Dir2;
    public float _attack1Cooldown = 0f;

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10151;
        _flameController = GetComponentInChildren<DragonFlameController>();
        isBoss = true;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Dragon_Attack1(this));
    }

    protected override void Update()
    {
        base.Update();
        if (_player != null && Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged && _attack1Cooldown <= 0f)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        if (_attack1Cooldown <= 3f && _attack1Cooldown > 0f)
        {
            _attack1Cooldown -= Time.deltaTime;
        }

    }

    public void GetAttack1Dir1()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_DragonAttack);
        _attack1Dir1 = (_player.transform.position - _mergePoint.transform.position).normalized;
        _attack1Cooldown = 3f;
        _coAttack1 = StartCoroutine(CoAttack1());
    }

    public void GetAttack1Dir2()
    {
        _attack1Dir2 = (_player.transform.position - _mergePoint.transform.position).normalized;
        float angle = 15f;
        int random = Random.Range(0, 2);
        if (random == 1)
        {
            angle *= -1;
        }
        float rad = angle * Mathf.Deg2Rad;

        float rotatedX = _attack1Dir2.x * Mathf.Cos(rad) - _attack1Dir2.y * Mathf.Sin(rad);
        float rotatedY = _attack1Dir2.x * Mathf.Sin(rad) + _attack1Dir2.y * Mathf.Cos(rad);

        _attack1Dir2 = new Vector2(rotatedX, rotatedY);
    }

    private IEnumerator CoAttack1()
    {
        yield return _attackDelay;
        _movement._isTrace = true;
        _isAttack1 = false;
    }

    public void ShootFlame()
    {
        _flameController.ShootFlame(_attack1Dir1, _model._attack1Damage);
        _flameController.ShootFlame(_attack1Dir2, _model._attack1Damage);
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_DragonDie);
    }
}
