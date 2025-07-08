using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    static PlayerStatManager instance;
    int _damageRand;
    int _luckRand;
    float _attackSpeedRand;

    // �ʱ� PlayerStatManager ����
    public static PlayerStatManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("PlayerStatManager");
                instance = gameObject.AddComponent<PlayerStatManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        CreatePlayerStatManager();
        _damageRand = Random.Range(8, 14);
        _luckRand = Random.Range(0, 3);
        _attackSpeedRand = Random.Range(1.6f, 2.0f);

        _attackDamage = _damageRand;
        _attackSpeed = _attackSpeedRand;
        _playerLuck = _luckRand;
    }

    private void CreatePlayerStatManager()
    {
        if (instance == null)   // ������ �Ǿ� ������ �� ������ �ʰ� ���
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else    // �ߺ� ����
        {
            Destroy(gameObject);
        }
    }

    

    [Header("Player Status")]
    // Player HP
    [SerializeField] float playerHp = 6;
    public float _playerHp { get { return playerHp; } set { playerHp = value; } }

    // Player Max Hp (�б� ����)
    [SerializeField] float playerMaxHp = 24;
    public float _playerMaxHp { get { return playerMaxHp; } }

    // Player Attack Damage
    [SerializeField] int attackDamage;
    public int _attackDamage { get { return attackDamage; } set { attackDamage = value; } }

    // Player Move Speed
    [SerializeField][Range(0, 10)] float moveSpeed = 5f;
    public float _moveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    // Player Acceleration Speed
    [SerializeField][Range(0, 30)] float accelerationSpeed = 20f;
    public float _accelerationSpeed { get { return accelerationSpeed; } set { accelerationSpeed = value; } }

    // Player Deceleration Speed
    [SerializeField][Range(0, 30)] float decelerationSpeed = 20f;
    public float _decelerationSpeed { get { return decelerationSpeed; } set { decelerationSpeed = value; } }

    // Player Dash Speed
    [SerializeField][Range(5, 20)] int dashSpeed = 15;
    public int _dashSpeed { get { return dashSpeed; } set { dashSpeed = value; } }

    // Player Dash Duration Time
    [SerializeField][Range(0, 1)] float dashDurationTime = 0.2f;
    public float _dashDurationTime { get { return dashDurationTime; } set { dashDurationTime = value; } }

    // Player Dash CoolTime
    [SerializeField][Range(0, 2)] float dashCoolTime = 1f;
    public float _dashCoolTime { get { return dashCoolTime; } set { dashCoolTime = value; } }

    // Player Attack Speed
    [SerializeField] float attackRange = 0.8f;
    public float _attackRange { get { return attackRange; } set { attackRange = value; } }

    // Player Attack Speed
    [SerializeField] float attackSpeed;
    public float _attackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }

    // Player Dash Ability Check
    [SerializeField] bool canDash = false;
    public bool _canDash { get { return canDash; } set { canDash = value; } }

    // Player Luck
    [SerializeField] int playerLuck;
    public int _playerLuck { get { return playerLuck; } set { playerLuck = value; } }

    // Player Alive
    [SerializeField] bool alive = true;
    public bool _alive { get { return alive; } set { alive = value; } }

    // Player Can Resurrect
    [SerializeField] bool canResurrect = false;
    public bool _canResurrect { get { return canResurrect; } set { canResurrect = value; } }

    // Additional Drop Gold
    [SerializeField] int additionalDropGold = 0;
    public int _additionalDropGold { get { return additionalDropGold; } set { additionalDropGold = value; } }

    [SerializeField] float shield = 0f;
    public float _shield { get { return shield; } set { shield = value; } }

    [SerializeField] bool isParry = false;
    public bool _isParry { get { return isParry; } set { isParry = value; } }

    [SerializeField] float attackBonus = 0f;
    public float _attackBonus { get { return attackBonus; } set { attackBonus = value; } }
}
