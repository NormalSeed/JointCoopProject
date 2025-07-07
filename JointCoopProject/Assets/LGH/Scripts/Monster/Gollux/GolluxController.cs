using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolluxController : MonsterBase
{
    private readonly WaitForSeconds _attackStart = new WaitForSeconds(0.5f);
    private readonly WaitForSeconds _attackDuration = new WaitForSeconds(2.5f);
    private Coroutine _coAttack1;
    
    public float _attackCooldown = 0f;
    public Vector2 _rushDestination = Vector2.zero;

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");
    public readonly int STUN_HASH = Animator.StringToHash("Stun");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10152;
        isBoss = true;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new Gollux_Attack1(this));
        _stateMachine._stateDic.Add(EState.Stun, new Gollux_Stun(this));
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
        if (_attackCooldown <= 2f && _attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }

        if (_attackCooldown <= 0f && !_isDamaged)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }
    }

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
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_GolluxAttack);
        _view.PlayAnimation(IDLE_HASH);
        Vector2 attackDir = (_player.transform.position - transform.position).normalized;
        _rushDestination = (Vector2)transform.position + attackDir * _model._attack2Range;

        yield return _attackStart;
        _view.PlayAnimation(ATTACK1_HASH);
        _model._bodyDamage = 2;

        yield return _attackDuration;
        _attackCooldown = 2f;
        _movement._isTrace = true;
        _isAttack1 = false;
        _rushDestination = Vector2.zero;
        _model._bodyDamage = 1;
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_GolluxDie);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage1);
    }

    private void OnDisable()
    {
        if (SoundManager.Instance.audioBgm != null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage1);
        }
        else
        {
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage1);
        }
    }
}
