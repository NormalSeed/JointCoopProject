using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrayCatController : MonsterBase
{
    [SerializeField] private GameObject _mergePoint;
    private SpriteRenderer _spriteRenderer;
    private StrayCatBulletController _bulletController;

    public int _attackType;
    public float _typeChangeDelay = 4f;

    private Coroutine _coAttack1;
    private readonly WaitForSeconds _bulletDelay = new WaitForSeconds(0.2f);
    private readonly WaitForSeconds _attack1Delay = new WaitForSeconds(0.1f);
    private Vector2 _attack1Dir;

    [SerializeField] GameObject _grenade;
    private Coroutine _coAttack2;
    private readonly WaitForSeconds _attack2Delay = new WaitForSeconds(1.5f);

    public Vector2 _attack3Dir1;
    public Vector2 _attack3Dir2;
    public Vector2 _rushDestination1;
    public Vector2 _rushDestination2;
    private Coroutine _coAttack3;
    private readonly WaitForSeconds _attack3DirChange = new WaitForSeconds(0.9f);
    private readonly WaitForSeconds _attack3Delay = new WaitForSeconds(3f);

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");
    public readonly int ATTACK2_HASH = Animator.StringToHash("Attack2");
    public readonly int ATTACK3_HASH = Animator.StringToHash("Attack3");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10401;

        _bulletController = GetComponentInChildren<StrayCatBulletController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _attackType = Random.Range(1, 4);
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new StrayCat_Attack1(this));
        _stateMachine._stateDic.Add(EState.Attack2, new StrayCat_Attack2(this));
        _stateMachine._stateDic.Add(EState.Attack3, new StrayCat_Attack3(this));
    }

    protected override void Update()
    {
        base.Update();
        if (_typeChangeDelay > 0 && _typeChangeDelay <= 4f) _typeChangeDelay -= Time.deltaTime;

        if (_typeChangeDelay <= 0)
        {
            _attackType = Random.Range(1, 4);
            _typeChangeDelay = 5f;
        }

        if (_attackType == 1)
        {
            _isAttack1 = true;
            _movement._isTrace = false;
        }
        else if (_attackType == 2)
        {
            _isAttack2 = true;
            _movement._isTrace = false;
        }
        else if (_attackType == 3)
        {
            _isAttack3 = true;
            _movement._isTrace = false;
        }
    }

    // Attack1
    public void Attack1()
    {
        _attackType = 0;

        GetAttack1Dir();
        if (_spriteRenderer.flipX)
        {
            _mergePoint.transform.position = (Vector2)_mergePoint.transform.position + new Vector2(-0.5f, 0);
        }
        else
        {
            _mergePoint.transform.position = (Vector2)_mergePoint.transform.position + new Vector2(0.5f, 0);
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

    public void GetAttack1Dir()
    {
        _attack1Dir = (_player.transform.position - _mergePoint.transform.position).normalized;
    }

    private IEnumerator CoAttack1()
    {
        int bulletCount = 0;

        while (bulletCount < 8)
        {
            _bulletController.ShootFire(_attack1Dir, _model._attack1Damage);
            yield return _bulletDelay;
            bulletCount++;
        }
        

        yield return _attack1Delay;
        _isAttack1 = false;
        _movement._isTrace = true;
    }

    // Attack2
    public void SetAttack2()
    {
        _attackType = 0;

        if (_coAttack2 != null)
        { 
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
        yield return _attack2Delay;
        _isAttack2 = false;
        _movement._isTrace = true;
    }

    public void Attack2()
    {
        SpawnExplosion();
        SpawnExplosion();
        SpawnExplosion();
    }

    public void SpawnExplosion()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized * Random.Range(0f, _model._attack2Range);
        Vector2 offset = new Vector2(randomDir.x, randomDir.y);
        Vector2 spawnPos = (Vector2)transform.position + offset;

        Instantiate(_grenade, spawnPos, Quaternion.identity);
    }

    // Attack3
    public void Attack3()
    {
        _attackType = 0;

        if (_coAttack1 != null)
        {
            _coAttack1 = null;
            _coAttack1 = StartCoroutine(CoAttack3());
        }
        else
        {
            _coAttack1 = StartCoroutine(CoAttack3());
        }
    }

    private void GetAttack1Dir1()
    {
        _attack3Dir1 = (_player.transform.position - transform.position).normalized;
        _rushDestination1 = _attack3Dir1 * _model._attack3Range;
    }

    private void GetAttack1Dir2()
    {
        _attack3Dir2 = Random.insideUnitCircle.normalized;
        Vector2 rushDestination = _attack3Dir2 * _model._attack3Range;
        _rushDestination2 = (Vector2)transform.position + rushDestination;
    }

    private IEnumerator CoAttack3()
    {
        GetAttack1Dir1();
        yield return _attack3DirChange;
        GetAttack1Dir2();
        yield return _attack3Delay;

        _movement._isTrace = true;
        _isAttack3 = false;
    }
}
