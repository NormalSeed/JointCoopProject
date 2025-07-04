using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherController : MonsterBase
{
    [SerializeField] private Transform _mergePoint;
    [SerializeField] private SkeletonArcherArrowController _arrowController;
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(1f);
    public Vector2 _attack1Dir;
    public float _attack1Cooldown = 0f;

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10107;
        _arrowController = GetComponentInChildren<SkeletonArcherArrowController>();
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new SkeletonArcher_Attack1(this));
    }

    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged && _attack1Cooldown <= 0f)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        if (_attack1Cooldown <= 5f && _attack1Cooldown > 0f)
        {
            _attack1Cooldown -= Time.deltaTime;
        }

    }

    public void GetAttack1Dir()
    {
        _attack1Dir = (_player.transform.position - transform.position).normalized;
        _attack1Cooldown = 5f;
        _coAttack1 = StartCoroutine(CoAttack1());
    }

    private IEnumerator CoAttack1()
    {
        yield return _attackDelay;
        _movement._isTrace = true;
        _isAttack1 = false;
    }

    public void ShootArrow()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_SkeletonArcherAttack);
        _arrowController.ShootArrow(_attack1Dir, _model._attack1Damage);
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_SkeletonDie);
    }
}
