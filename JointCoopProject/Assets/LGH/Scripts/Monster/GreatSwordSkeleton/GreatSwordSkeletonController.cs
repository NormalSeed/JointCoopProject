using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordSkeletonController : MonsterBase
{
    [SerializeField] private Collider2D _attackCollider;
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(1f);

    public readonly int ATTACK1_1_HASH = Animator.StringToHash("Attack1_1");
    public readonly int ATTACK1_2_HASH = Animator.StringToHash("Attack1_2");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10203;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new GreatSwordSkeleton_Attack1(this));
    }

    protected override void Update()
    {
        base.Update();
        if (_player != null && Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }
    }

    public void Attack1()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_GreatSwordSkeletonAttack);
        Vector2 attackDir = _player.transform.position - transform.position;
        float xDir = attackDir.x;

        if (xDir < 0f)
        {
            _attackCollider.transform.position = transform.position + new Vector3(-_model._attack1Range / 2, 0);
        }
        else if (xDir > 0f)
        {
            _attackCollider.transform.position = transform.position + new Vector3(_model._attack1Range / 2, 0);
        }

        // ¿¸πÊ¿∏∑Œ µµ≥¢∏¶ »÷µŒ∏• »ƒ 1√  ∏ÿ√„
        // 1√  ∏ÿ√Áæﬂ «œπ«∑Œ Coroutine ªÁøÎ « ø‰
        _coAttack1 = StartCoroutine(CoAttack1());
    }

    private IEnumerator CoAttack1()
    {
        yield return _attackDelay;
        _movement._isTrace = true;
        _isAttack1 = false;
    }

    public void EnableAttackCollider()
    {
        _attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        _attackCollider.enabled = false;
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_SkeletonDie);
    }
}
