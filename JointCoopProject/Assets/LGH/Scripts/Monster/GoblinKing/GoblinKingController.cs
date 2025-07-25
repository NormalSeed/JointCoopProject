using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingController : MonsterBase
{
    [SerializeField] GameObject _attack1AoE; // 장판 프리팹(십자형)
    [SerializeField] GameObject _attack1Warn; // 경고 프리팹
    private Coroutine _coAttack1;
    private readonly WaitForSeconds _attack1Delay = new WaitForSeconds(1.05f);
    private readonly WaitForSeconds _attack1Duration = new WaitForSeconds(0.2f);
    private readonly WaitForSeconds _attack1EndTime = new WaitForSeconds(3f);
    private float _attack1Cooldown = 1f;

    [SerializeField] public GameObject _attack2Collider;
    [SerializeField] public GameObject _attack2Warn;
    private Coroutine _coAttack2;
    private readonly WaitForSeconds _attack2EndTime = new WaitForSeconds(4f);

    public readonly int ATTACK1_HASH = Animator.StringToHash("Attack1");
    public readonly int ATTACK2_HASH = Animator.StringToHash("Attack2");

    protected override void Init()
    {
        base.Init();
        _monsterID = 10352;
        isBoss = true;
    }

    protected override void StateMachineInit()
    {
        base.StateMachineInit();
        _stateMachine._stateDic.Add(EState.Attack1, new GoblinKing_Attack1(this));
        _stateMachine._stateDic.Add(EState.Attack2, new GoblinKing_Attack2(this));
    }

    protected override void Update()
    {
        base.Update();
        
        if (_attack1Cooldown > 0f && !_isDamaged)
        {
            _attack1Cooldown -= Time.deltaTime;
        }

        if (_attack1Cooldown <= 0f)
        {
            _movement._isTrace = false;
            _isAttack1 = true;
        }
    }

    protected override void PlayBossBGM()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_BossStage);
    }

    // Attack1 : 십자, X자가 번갈아 나오는 장판형 공격.
    // 두 장판 사이의 간격은 2초.
    public void Attack1()
    {
        if (_coAttack1 != null)
        {
            StopCoroutine(_coAttack1);
            _coAttack1 = null;
            StartCoroutine(CoAttack1());
        }
        else
        {
            StartCoroutine(CoAttack1());
        }
    }

    private IEnumerator CoAttack1()
    {
        _attack1Warn.SetActive(true);

        yield return _attack1Delay;
        _attack1Warn.SetActive(false);
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_GoblinKingAttack1);
        _attack1AoE.SetActive(true);

        yield return _attack1Duration;
        _attack1AoE.SetActive(false);
        _attack1Warn.transform.rotation = Quaternion.Euler(0, 0, 45);
        _attack1Warn.SetActive(true);

        yield return _attack1Delay;
        _attack1Warn.transform.rotation = Quaternion.identity;
        _attack1Warn.SetActive(false);
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_GoblinKingAttack1);
        _attack1AoE.transform.rotation = Quaternion.Euler(0, 0, 45);
        _attack1AoE.SetActive(true);

        yield return _attack1Duration;
        _attack1AoE.transform.rotation = Quaternion.identity;
        _attack1AoE.SetActive(false);

        yield return _attack1EndTime;
        _isAttack1 = false;
        _isAttack2 = true;
        _attack1Cooldown = 10f;
    }

    // Attack2 : 넉백 공격, 원형 콜라이더를 생성하고 TriggerEnter 됐을 때 데미지를 주며 넉백
    public void Attack2()
    {
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

    private IEnumerator CoAttack2()
    {
        yield return _attack1Delay;
        _view.PlayAnimation(IDLE_HASH);
        yield return _attack2EndTime;
        _isAttack2 = false;
        _isAttack1 = true;
    }

    public void ActivateAttack2Coll()
    {
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_GoblinKingAttack2);
        _attack2Warn.SetActive(false);
        _attack2Collider.SetActive(true);
    }

    public void DeactivateAttack2Coll()
    {
        _attack2Collider.SetActive(false);
    }

    public void ActivateAttack2Warn()
    {
        _attack2Warn.SetActive(true);
    }

    public override void Die()
    {
        base.Die();
        SoundManager.Instance.PlaySFX(SoundManager.ESfx.SFX_GoblinKingDie);
    }

    private void OnDisable()
    {
        if (SoundManager.Instance.audioBgm != null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage3);
        }
        else
        {
            SoundManager.Instance.PlayBGM(SoundManager.EBgm.BGM_Stage3);
        }
    }
}
