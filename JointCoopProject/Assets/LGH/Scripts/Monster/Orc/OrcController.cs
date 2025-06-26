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
        _monsterID = 10101;
        // csv 파일로 데이터를 받아오는 기능 구현 시도
        //_model._maxHP = 30;
        //_model._moveSpd = 2f;
        //_model._attack1Damage = 1;
        //_model._attack1Range = 1f;

    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Orc_Attack1(this));
    }

    protected override void Start()
    {
        base.Start();
        
        if (_dataDic.TryGetValue(_monsterID, out MonsterData data))
        {
            _model.ApplyData(data);
        }
        _model._curHP.Value = _model._maxHP;
    }

    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }

        //// TakeDamage 테스트
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(10, transform.position);
        //}
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

            // 전방으로 도끼를 휘두른 후 1초 멈춤
            // 1초 멈춰야 하므로 Coroutine 사용 필요
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
