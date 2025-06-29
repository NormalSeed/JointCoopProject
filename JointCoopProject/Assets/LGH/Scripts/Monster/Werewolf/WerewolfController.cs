using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfController : MonsterBase
{
    [SerializeField] private Collider2D _attackCollider;
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attackstart = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(2f);
    public float _attack1Cooldown = 0f;

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10205;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Werewolf_Attack1(this));
    }

    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged && _attack1Cooldown <= 0f)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        if (_attack1Cooldown <= 3f && _attack1Cooldown > 0f)
        {
            _attack1Cooldown -= Time.deltaTime;
        }

        //// TakeDamage Å×½ºÆ®
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(10, transform.position);
        //}
    }

    public void Attack1()
    {
        _coAttack1 = StartCoroutine(CoAttack1());
    }

    private IEnumerator CoAttack1()
    {
        yield return _attackstart;
        Vector2 attackDir = _player.transform.position - transform.position;
        Transform attackTransform = _player.transform;
        float xDir = attackDir.x;

        if (xDir < 0f)
        {
            transform.position = attackTransform.position + new Vector3(0.6f, 0);
            _attackCollider.transform.position = transform.position + new Vector3(-0.6f, 0);
        }
        else if (xDir > 0f)
        {
            transform.position = attackTransform.position - new Vector3(0.6f, 0);
            _attackCollider.transform.position = transform.position + new Vector3(0.6f, 0);
        }
        yield return _attackDelay;
        _attack1Cooldown = 3f;
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
