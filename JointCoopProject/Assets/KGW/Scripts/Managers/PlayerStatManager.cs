using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    static PlayerStatManager instance;

    // 초기 PlayerStatManager 생성
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
    }

    private void CreatePlayerStatManager()
    {
        if (instance == null)   // 생성이 되어 있으면 더 만들지 않고 사용
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else    // 중복 삭제
        {
            Destroy(gameObject);
        }
    }

    [Header("Player Status")]
    // Player HP
    [SerializeField] int playerHp = 6;
    public int _playerHp { get { return playerHp; } set { playerHp = value; } }

    // Player Max Hp (읽기 전용)
    [SerializeField] int playerMaxHp = 13;
    public int _playerMaxHp { get { return playerMaxHp; } }

    // Player Attack Damage
    [SerializeField] int attackDamage = 3;
    public int _attackDamage { get { return attackDamage; } set { attackDamage = value; } }

    // Player Move Speed
    [SerializeField][Range(0, 10)] float moveSpeed = 3f;
    public float _moveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    // Player Acceleration Speed
    [SerializeField][Range(0, 30)] float accelerationSpeed = 20f;
    public float _accelerationSpeed { get { return accelerationSpeed; } set { accelerationSpeed = value; } }

    // Player Deceleration Speed
    [SerializeField][Range(0, 30)] float decelerationSpeed = 20f;
    public float _decelerationSpeed { get {return decelerationSpeed; } set { decelerationSpeed = value; } }

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
    [SerializeField] float attackSpeed = 1f;
    public float _attackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }

    // Player Dash Ability Check
    [SerializeField] bool canDash = false;
    public bool _canDash { get { return canDash; } set { canDash = value; } }

    // Player Luck
    [SerializeField] int playerLuck = 0;
    public int _playerLuck { get { return playerLuck; } set { playerLuck = value; } }

    // Player Alive
    [SerializeField] bool alive = true;
    public bool _alive { get { return alive; } set { alive = value; } }

}
