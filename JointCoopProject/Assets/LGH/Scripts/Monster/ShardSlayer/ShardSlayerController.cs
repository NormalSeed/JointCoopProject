using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardSlayerController : MonsterBase
{
    private ShardSlayerFireController _fireController;

    public int _attackType;

    // Attack1 관련 필드
    public Vector2 _attack1Dir1;
    public Vector2 _attack1Dir2;
    public Vector2 _rushDestination1;
    public Vector2 _rushDestination2;
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attack1DirChange = new WaitForSeconds(0.9f);
    private readonly WaitForSeconds _attackDelay = new WaitForSeconds(3f);

    // Attack2 관련 필드
    public Vector2 _attack2Dir1;
    public Vector2 _attack2Dir2;
    private Coroutine _coAttack2;
    private readonly WaitForSeconds _attack2Delay = new WaitForSeconds(2f);

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");
    public readonly int ATTACK2_HASH = Animator.StringToHash("Attack2");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10251;

        _fireController = GetComponentInChildren<ShardSlayerFireController>();
        
        _attackType = 1;
        isBoss = true;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new ShardSlayer_Attack1(this));
        _stateMachine._stateDic.Add(EState.Attack2, new ShardSlayer_Attack2(this));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_BossStage);
    }

    protected override void Update()
    {
        base.Update();
        if (_player != null && Vector2.Distance(transform.position, _player.transform.position) <= _model._attack1Range && !_isDamaged && _attackType == 1)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }
        if (_player != null && Vector2.Distance(transform.position, _player.transform.position) <= _model._attack2Range && !_isDamaged && _attackType == 2)
        {
            _movement._isTrace = false;
            _isAttack2 = true;
        }
    }

    // Attack1 : 돌진 공격, 사거리 6, 감지한 캐릭터의 방향으로 6만큼 돌진 후 랜덤 방향으로 6만큼 추가 돌진
    // 추가 돌진이 끝난 후 2초간 멈춤
    // 멈춘 후에 _attackType을 2로 초기화
    public void Attack1()
    {
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

    private void GetAttack1Dir1()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_ShardSlayerAttack1);
        _attack1Dir1 = (_player.transform.position - transform.position).normalized;
        _rushDestination1 = (Vector2)transform.position + _attack1Dir1 * _model._attack1Range;
    }

    private void GetAttack1Dir2()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_ShardSlayerAttack1);
        _attack1Dir2 = Random.insideUnitCircle.normalized;
        Vector2 rushDestination = _attack1Dir2 * _model._attack1Range;
        _rushDestination2 = (Vector2)transform.position + rushDestination;
    }

    private IEnumerator CoAttack1()
    {
        GetAttack1Dir1();
        yield return _attack1DirChange;
        GetAttack1Dir2();
        yield return _attackDelay;

        _movement._isTrace = true;
        _isAttack1 = false;
    }

    // Attack2 : 투사체 발사, 총 2발, 발사 후 2초 대기
    public void Attack2()
    {
        GetAttack2Dir();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_ShardSlayerAttack2);
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
        _movement._isTrace = true;
        _isAttack2 = false;
    }

    public void GetAttack2Dir()
    {
        Vector2 attack2Dir = (_player.transform.position - transform.position).normalized;
        float angle = 15f;
        float rad = angle * Mathf.Deg2Rad;

        float rotated1X = attack2Dir.x * Mathf.Cos(rad) - attack2Dir.y * Mathf.Sin(rad);
        float rotated2X = attack2Dir.x * Mathf.Cos(-rad) - attack2Dir.y * Mathf.Sin(-rad);
        float rotated1Y = attack2Dir.x * Mathf.Sin(rad) + attack2Dir.y * Mathf.Cos(rad);
        float rotated2Y = attack2Dir.x * Mathf.Sin(-rad) + attack2Dir.y * Mathf.Cos(-rad);
        _attack2Dir1 = new Vector2(rotated1X, rotated1Y);
        _attack2Dir2 = new Vector2(rotated2X, rotated2Y);
    }

    public void ShootFire()
    {
        _fireController.ShootFire(_attack2Dir1, _model._attack2Damage);
        _fireController.ShootFire(_attack2Dir2, _model._attack2Damage);
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_ShardSlayerDie);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage2);
    }

    private void OnDisable()
    {
        if (SoundManager.Instance.audioBgm != null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage2);
        }
        else
        {
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage2);
        }
    }
}
