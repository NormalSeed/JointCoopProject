using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkeletonController : MonsterBase
{
    [SerializeField] private Collider2D _attack1Collider;
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(1f);

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10108;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Skeleton_Attack1(this));
    }

    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }
    }

    public void Attack1()
    {
        Vector2 attackDir = _player.transform.position - transform.position;
        float xDir = attackDir.x;

        if (xDir < 0f)
        {
            _attack1Collider.transform.position = (Vector2)transform.position + new Vector2(-_model._attack1Range / 2, 0);
        }
        else if (xDir > 0f)
        {
            _attack1Collider.transform.position = (Vector2)transform.position + new Vector2(_model._attack1Range / 2, 0);
        }

        if (_coAttack1 != null)
        {
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
        yield return _attackDelay;
        _movement._isTrace = true;
        _isAttack1 = false;
    }

    public void EnableAttack1Collider()
    {
        _attack1Collider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        _attack1Collider.enabled = false;
    }
}
