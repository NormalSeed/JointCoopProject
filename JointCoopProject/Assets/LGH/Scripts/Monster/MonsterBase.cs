using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
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
    private float _activeDelay;
    public bool _isActivated;
    public MonsterMovement _movement;
    public MonsterModel _model;
    public MonsterView _view;
    public StateMachine _stateMachine;
    public bool _isAttack1;

    public readonly int IDLE_HASH = Animator.StringToHash("Idle");
    public readonly int MOVE_HASH = Animator.StringToHash("Walk");

    private void Awake() => Init();

    protected virtual void Init()
    {
        _model = GetComponent<MonsterModel>();
        _movement = GetComponent<MonsterMovement>();
        _view = GetComponent<MonsterView>();

        StateMachineInit();
    }

    // 모든 Monster들이 공통적으로 가질 Idle, Patrol
    protected virtual void StateMachineInit()
    {
        _stateMachine = new StateMachine();
        _stateMachine._stateDic.Add(EState.Idle, new Monster_Idle(this));
        _stateMachine._stateDic.Add(EState.Patrol, new Monster_Patrol(this));
        _stateMachine._stateDic.Add(EState.Trace, new Monster_Trace(this));

        _stateMachine._curState = _stateMachine._stateDic[EState.Idle];
    }

    private void Start()
    {
        _activeDelay = 1f;
        _isActivated = false;
        _isAttack1 = false;
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
}
