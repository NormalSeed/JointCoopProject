using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcController : MonsterBase
{
    [SerializeField] private Collider2D _attackCollider;
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(1f);

    public readonly int ATTACK1_HASH = Animator.StringToHash("OrcAttack1");

    protected override void Init()
    {
        base.Init();
        _model._maxHP = 30;
        _model._curHP.Value = _model._maxHP;
        _model._attack1Damage = 1;
        _model._attackRange = 1f;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Orc_Attack1(this));
    }

    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, _player.transform.position) <= _model._attackRange)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        // TakeDamage ≈◊Ω∫∆Æ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1, transform.position);
        }
    }

    public void Attack1()
    {
        Vector2 attackDir = _player.transform.position - transform.position;
        float xDir = attackDir.x;

        if (xDir < 0f)
        {
            _attackCollider.transform.position = transform.position + new Vector3(-1f, 0);
        }
        else if (xDir > 0f)
        {
            _attackCollider.transform.position = transform.position + new Vector3(1f, 0);
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
}
