using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncubusController : MonsterBase
{
    [SerializeField] private Collider2D _attack1Collider;
    private Coroutine _coAttack1;
    private WaitForSeconds _attack1Delay = new WaitForSeconds(1f);

    private float _innerRadius;
    private float _outerRadius;
    private Coroutine _coAttack2;
    private WaitForSeconds _attack2Delay = new WaitForSeconds(1f);
    public float _attack2Cooldown = 10f;

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");
    public readonly int ATTACK2_HASH = Animator.StringToHash("Attack2");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10351;
        isBoss = true;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Incubus_Attack1(this));
        _stateMachine._stateDic.Add(EState.Attack2, new Incubus_Attack2(this));
    }

    protected override void Update()
    {
        base.Update();
        if (_attack2Cooldown > 0f)
        {
            _attack2Cooldown -= Time.deltaTime;
        }

        if (_player != null && Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged && !_isAttack2)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        if (!_isDamaged && _attack2Cooldown <= 0f && !_isAttack1)
        {
            _movement._isTrace = false;
            _isAttack2 = true;
        }
    }

    // Attack1: 기본공격
    public void Attack1()
    {
        Vector2 attackDir = _player.transform.position - transform.position;
        float xDir = attackDir.x;

        if (xDir < 0f)
        {
            _attack1Collider.transform.position = transform.position + new Vector3(-_model._attack1Range / 2, 0);
        }
        else if (xDir > 0f)
        {
            _attack1Collider.transform.position = transform.position + new Vector3(_model._attack1Range / 2, 0);
        }

        // 전방으로 채찍을 휘두른 후 1초 멈춤
        // 1초 멈춰야 하므로 Coroutine 사용 필요
        _coAttack1 = StartCoroutine(CoAttack1());
    }

    private IEnumerator CoAttack1()
    {
        yield return _attack1Delay;
        _movement._isTrace = true;
        _isAttack1 = false;
    }

    public void EnableAttackCollider()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_IncubusAttack1);
        _attack1Collider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        _attack1Collider.enabled = false;
    }

    // Attack2: 회전공격하며 이동, 쿨타임 10초
    public void Attack2()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_IncubusAttack2);
        _model._moveSpd /= 2;

        if (_coAttack2 != null)
        {
            StopCoroutine(CoAttack2());
            _coAttack2 = null;
            _coAttack2 = StartCoroutine(CoAttack2());
        }
        else
        {
            _coAttack2 = StartCoroutine(CoAttack2());
        }
    }

    private IEnumerator CoAttack2()
    {
        float duration = 4f; // 공격 지속시간
        float interval = 1f; // 공격 간격
        float elapsed = 0f; // 지난 시간

        while (elapsed < duration)
        {
            float distance = Vector2.Distance(transform.position, _player.transform.position);
            _outerRadius = _model._attack2Range;
            _innerRadius = _model._attack2Range - 2;

            if (distance >= _innerRadius && distance <= _outerRadius)
            {
                _player.GetComponent<IDamagable>()?.TakeDamage(_model._attack2Damage, _player.transform.position);
            }

            yield return _attack2Delay;
            elapsed += interval;
        }
        _movement._isTrace = true;
        _model._moveSpd *= 2;
        _isAttack2 = false;
        _attack2Cooldown = 10f;
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_IncubusDie);
    }
}
