using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour, IDamagable
{
    // FSM을 통해 움직이는 Monster들의 기본
    // StateMachine을 받아오고
    // 상태 변경을 StateMachine에 전달해주는 역할
    // model, view, movement 컴포넌트 가짐
    // movement는 MonsterMovement로 모든 몬스터들의 기본적인 상하좌우 움직임 AI를 가져야 함
    // model은 몬스터의 스탯(체력, 공격력, 이동속도, 사정거리 등)
    // view는 몬스터 애니메이션, 체력 UI 등 사용자가 직접 눈으로 볼 수 있는 요소들
    // 모든 몬스터는 플레이어가 방에 처음 진입했을 때 1초의 딜레이를 갖고 활성화된다.
    // 방에 들어갔을 때 SetActive(true) 되고 Start에서 1초 대기를 걸어주면 된다.
    // 데미지를 받을 수 있고 줄 수 있음
    // curHP가 0이 되면 사망 처리
    // TODO: 사망시 랜덤 확률로 재화 또는 아이템 드롭
    private float _activeDelay;
    public bool _isActivated;
    public MonsterMovement _movement;
    public MonsterModel _model;
    public MonsterView _view;
    public StateMachine _stateMachine;
    public GameObject _player;
    public bool _isAttack1;
    public bool _isAttack2;
    public bool _isDamaged;
    public bool _isDead;
    private Coroutine _coOffDamage;
    private WaitForSeconds _damageDelay = new WaitForSeconds(1f);

    public readonly int IDLE_HASH = Animator.StringToHash("Idle");
    public readonly int MOVE_HASH = Animator.StringToHash("Walk");
    public readonly int DAMAGED_HASH = Animator.StringToHash("Damaged");
    public readonly int DEAD_HASH = Animator.StringToHash("Dead");


    private void Awake() => Init();

    protected virtual void Init()
    {
        _model = GetComponent<MonsterModel>();
        _movement = GetComponent<MonsterMovement>();
        _view = GetComponent<MonsterView>();
        _player = GameObject.Find("Player");

        StateMachineInit();
    }

    // 모든 Monster들이 공통적으로 가질 Idle, Patrol
    protected virtual void StateMachineInit()
    {
        _stateMachine = new StateMachine();
        _stateMachine._stateDic.Add(EState.Idle, new Monster_Idle(this));
        _stateMachine._stateDic.Add(EState.Patrol, new Monster_Patrol(this));
        _stateMachine._stateDic.Add(EState.Trace, new Monster_Trace(this));
        _stateMachine._stateDic.Add(EState.Damaged, new Monster_Damaged(this));
        _stateMachine._stateDic.Add(EState.Dead, new Monster_Dead(this));

        _stateMachine._curState = _stateMachine._stateDic[EState.Idle];
    }

    private void Start()
    {
        _activeDelay = 1f;
        _isActivated = false;
        _isAttack1 = false;
        _isAttack2 = false;
        _isDamaged = false;
        _isDead = false;
    }

    protected virtual void Update()
    {
        _activeDelay -= Time.deltaTime;
        if (_activeDelay < 0f)
        {
            _isActivated = true;
            _movement._isTrace = true;
        }
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(1, transform.position);
            }
        }
    }

    public void TakeDamage(int damage, Vector2 targetPos)
    {
        if (_isDamaged) return;

        _movement._isTrace = false;
        _isAttack1 = false;
        _isDamaged = true;
        _model._curHP.Value -= damage;

        if (_model._curHP.Value <= 0)
        {
            Die();
        }

        _coOffDamage = StartCoroutine(CoOffDamage());
    }

    private IEnumerator CoOffDamage()
    {
        yield return _damageDelay;
        _movement._isTrace = true;
        _isDamaged = false;
    }

    public void Die()
    {
        _isDead = true;
    }

    public void UnactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
