using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcRiderController : MonsterBase
{
    private SpriteRenderer _spriteRenderer;

    private readonly WaitForSeconds _attack1Delay = new WaitForSeconds(2.5f);
    private Coroutine _coAttack1;
    public float _attack1Cooldown = 5f;
    public Vector2 _rushDestination = Vector2.zero;

    private readonly WaitForSeconds _attack2Delay = new WaitForSeconds(1f);
    private Coroutine _coAttack2;
    private OrcRiderBulletController _bulletController;
    private float _attack2Cooldown = 0f;
    private Vector2 _attack2Dir;

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");
    public readonly int ATTACK2_HASH = Animator.StringToHash("Attack2");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10305;
        _bulletController = GetComponentInChildren<OrcRiderBulletController>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new OrcRider_Attack1(this));
        _stateMachine._stateDic.Add(EState.Attack2, new OrcRider_Attack2(this));
        _stateMachine._stateDic.Add(EState.Stun, new OrcRider_Stun(this));
    }

    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged && _attack1Cooldown <= 0f && !_isAttack2)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        if (_attack1Cooldown <= 5f && _attack1Cooldown > 0f)
        {
            _attack1Cooldown -= Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, _player.transform.position) <= _model._attack2Range && !_isDamaged && _attack2Cooldown <= 0f && !_isAttack1)
        {
            _movement._isTrace = false;
            _isAttack2 = true;
        }

        if (_attack2Cooldown <= 4f && _attack2Cooldown > 0f)
        {
            _attack2Cooldown -= Time.deltaTime;
        }
    }

    // Attack1 : 플레이어방향으로 1초간 4m 돌진, 돌진 후에 3초간 움직이지 못함, 쿨타임 5초
    public void Attack1()
    {
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
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_EliteOrcAttack);
        Vector2 attackDir = (_player.transform.position - transform.position).normalized;
        _rushDestination = (Vector2)transform.position + attackDir * _model._attack1Range;

        yield return _attack1Delay;

        _attack1Cooldown = 5f;
        _movement._isTrace = true;
        _isAttack1 = false;
        _rushDestination = Vector2.zero;
    }

    public void GetAttack2Dir()
    {
        _attack2Dir = (_player.transform.position - transform.position).normalized;
        if (_attack2Dir.x < 0f)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        if (_coAttack2 != null)
        {
            StopCoroutine(_coAttack2);
            _coAttack2 = null;
            _coAttack2 = StartCoroutine(CoAttack2());
        }
        else
        {
            _coAttack2 = StartCoroutine(CoAttack2());
        }
    }

    public void ShootBullet()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_OrcRiderThrow);
        _bulletController.ShootBullet(_attack2Dir, _model._attack2Damage);
    }

    private IEnumerator CoAttack2()
    {
        yield return _attack2Delay;

        _attack2Cooldown = 4f;
        _movement._isTrace = true;
        _isAttack2 = false;
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_EliteOrcDie);
    }
}
