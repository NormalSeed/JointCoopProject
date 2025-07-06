using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoredOrcController : MonsterBase
{
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(1.5f);
    private Coroutine _coAttack1;
    public float _attack1Cooldown = 0f;
    private Vector2 _attack1Dir;
    public Vector2 _rushDestination = Vector2.zero;
    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10304;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new ArmoredOrc_Attack1(this));
        _stateMachine._stateDic.Add(EState.Stun, new ArmoredOrc_Stun(this));
    }

    protected override void Update()
    {
        base.Update();
        if (_player != null && Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged && _attack1Cooldown <= 0f)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        if (_attack1Cooldown <= 6f && _attack1Cooldown > 0f)
        {
            _attack1Cooldown -= Time.deltaTime;
        }
    }

    // Attack1 : 플레이어방향으로 1초간 3m 돌진, 돌진 후에 3초간 움직이지 못함, 쿨타임 6초
    public void Attack1()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_EliteOrcAttack);
        if (_coAttack1 != null)
        {
            StopCoroutine(_coAttack1);
            _coAttack1 = null;
            _coAttack1 = StartCoroutine(CoAttack1());
        }
        else
        {
            _coAttack1 = StartCoroutine(CoAttack1());
        }
    }

    private IEnumerator CoAttack1()
    {
        _attack1Dir = (_player.transform.position - transform.position).normalized;
        _rushDestination = (Vector2)transform.position + _attack1Dir * _model._attack1Range;
        yield return _attackDelay;

        _attack1Cooldown = 6f;
        _movement._isTrace = true;
        _isAttack1 = false;
        _rushDestination = Vector2.zero;
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_EliteOrcDie);
    }
}
