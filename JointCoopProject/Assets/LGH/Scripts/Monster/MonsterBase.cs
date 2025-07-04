using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : MonoBehaviour, IDamagable
{
    // FSM�� ���� �����̴� Monster���� �⺻
    // StateMachine�� �޾ƿ���
    // ���� ������ StateMachine�� �������ִ� ����
    // model, view, movement ������Ʈ ����
    // movement�� MonsterMovement�� ��� ���͵��� �⺻���� �����¿� ������ AI�� ������ ��
    // model�� ������ ����(ü��, ���ݷ�, �̵��ӵ�, �����Ÿ� ��)
    // view�� ���� �ִϸ��̼�, ü�� UI �� ����ڰ� ���� ������ �� �� �ִ� ��ҵ�
    // ��� ���ʹ� �÷��̾ �濡 ó�� �������� �� 1���� �����̸� ���� Ȱ��ȭ�ȴ�.
    // �濡 ���� �� SetActive(true) �ǰ� Start���� 1�� ��⸦ �ɾ��ָ� �ȴ�.
    // �������� ���� �� �ְ� �� �� ����
    // curHP�� 0�� �Ǹ� ��� ó��
    // TODO: ����� ���� Ȯ���� ��ȭ �Ǵ� ������ ���
    protected int _monsterID;
    public float _activeDelay;
    public bool _isActivated;
    public MonsterMovement _movement;
    public MonsterModel _model;
    public Dictionary<int, MonsterData> _dataDic = new();
    public MonsterView _view;
    public StateMachine _stateMachine;
    public GameObject _player;
    public bool _isAttack1;
    public bool _isAttack2;
    public bool _isAttack3;
    public bool _isDamaged;
    public bool _isDead;
    private Coroutine _coOffDamage;
    private WaitForSeconds _damageDelay = new WaitForSeconds(1f);
    public bool isBoss = false;
    public Action OnBossDied;
    
    //Monster RoomManager
    private RoomMonsterManager roomMonsterManager;
    private Vector2Int myRoom;

    public readonly int IDLE_HASH = Animator.StringToHash("Idle");
    public readonly int MOVE_HASH = Animator.StringToHash("Walk");
    public readonly int DAMAGED_HASH = Animator.StringToHash("Damaged");
    public readonly int DEAD_HASH = Animator.StringToHash("Dead");


    private void Awake() => Init();

    /// <summary>
    /// �� monster controller���� _monsterID ���� �ʿ�
    /// </summary>
    protected virtual void Init()
    {
        _model = GetComponent<MonsterModel>();
        _movement = GetComponent<MonsterMovement>();
        _view = GetComponent<MonsterView>();
        _player = GameObject.Find("Player");

        roomMonsterManager = FindObjectOfType<RoomMonsterManager>();
        
        LoadCSV("MonsterStats");

        StateMachineInit();
    }

    /// <summary>
    /// ��� Monster���� ������ �⺻ ���� ����.
    /// �� monster controller���� ���� �߰��� ���
    /// </summary>
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

    protected void Start()
    {
        _activeDelay = 1f;
        _isActivated = false;
        _isAttack1 = false;
        _isAttack2 = false;
        _isAttack3 = false;
        _isDamaged = false;
        _isDead = false;

        if (_dataDic.TryGetValue(_monsterID, out MonsterData data))
        {
            _model.ApplyData(data);
        }
        _model._curHP.Value = _model._maxHP;

        FindMyRoom();
    }
    
    void FindMyRoom()
    {
        MapGenerator mapGen = FindObjectOfType<MapGenerator>();
        if (mapGen != null)
        {
            int x = Mathf.FloorToInt(transform.position.x / mapGen.prefabSize.x);
            int y = Mathf.FloorToInt(transform.position.y / mapGen.prefabSize.y);
            myRoom = new Vector2Int(x, y);
        }
    }

    private void LoadCSV(string path)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(path);
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');

            MonsterData monster = new MonsterData
            {
                _ID = int.Parse(values[0]),
                _name = values[1],
                _maxHP = int.Parse(values[2]),
                _bodyDamage = int.Parse(values[3]),
                _attack1Damage = int.Parse(values[4]),
                _attack2Damage = int.Parse(values[5]),
                _attack3Damage = int.Parse(values[6]),
                _attack1Range = float.Parse(values[7]),
                _attack2Range = float.Parse(values[8]),
                _attack3Range = float.Parse(values[9]),
                _moveSpd = float.Parse(values[10])
            };

            _dataDic.Add(monster._ID, monster);
        }
    }

    /// <summary>
    /// �� monster�� ���� �ִ� ���� ����(Attack1, Attack2 ��)�� ���� ��Ÿ� ��� �ʿ�
    /// </summary>
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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_model._bodyDamage, transform.position);
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

    public virtual void Die()
    {
        _isDead = true;
        
        if (roomMonsterManager != null)
        {
            roomMonsterManager.MonsterDied(this, myRoom);
        }
        if (isBoss)
        {
            OnBossDied?.Invoke();
        }
        
      
    }

    public void UnactivateSelf()
    {
        //TODO: ��Ȱ��ȭ�ϸ� ���ÿ� ������ �Ǵ� ��ȭ�� ����ϴ� ��� ���� �ʿ�
        if (_isDead)
        {
            gameObject.SetActive(false);
        }
    }
}
