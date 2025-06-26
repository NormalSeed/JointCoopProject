using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDamagable
{
    [Header("Player Weapon Status")]
    [SerializeField] GameObject _tearPrefab;
    [SerializeField] GameObject _swordPrefab;
    [SerializeField] float _tearAttackDelay = 0.3f;     // 눈물 공격 딜레이
    [SerializeField] float _swordAttackDelay = 0.5f;    // 근접 공격 딜레이
    [SerializeField] bool _isMeleeWeapon;

    PlayerStatus _playerStatus;
    SpriteRenderer _PlayerSprite;
    public Rigidbody2D _playerRigid;
    Animator _playerAnimator;

    Vector2 _moveInput;
    Vector2 _targetVelocity;
    public Vector2 _curVelocity;
    Vector2 _attackDirection;
    Vector2 _dashDirection;

    float _shotTimer;
    float _wieldTimer;   
    bool _isDash = false;
    bool _isDamaged = false;
    float _dashProgressTime;
    float _dashCoolTime;

    readonly int DASH_HASH = Animator.StringToHash("PlayerDash");
    readonly int DOWN_ATTACK_HASH = Animator.StringToHash("Player_Down_Attack");
    readonly int UP_ATTACK_HASH = Animator.StringToHash("Player_Up_Attack");
    readonly int LEFT_ATTACK_HASH = Animator.StringToHash("Player_Left_Attack");
    readonly int RIGHT_ATTACK_HASH = Animator.StringToHash("Player_Right_Attack");

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (_playerStatus._isAlive)
        {
            MoveInput();

            if (_playerStatus._canDash && _isDash)
            {
                MoveDash();
            }
            else
            {
                Movement();
                ShotInput();
                DashInput();
                Attack();
            }
        }
        else
        {
            _playerStatus.PlayerDeath();
        }
    }

    // First Initialize
    private void Init()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerStatus = GetComponent<PlayerStatus>();
        _playerAnimator = GetComponent<Animator>();
        _PlayerSprite = GetComponent<SpriteRenderer>();
        _isMeleeWeapon = true;
        _playerStatus._isAlive = true;
    }

    // Player Move Input
    private void MoveInput()
    {
        if (_playerStatus._isKnockBack) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _moveInput = new Vector2(horizontal, vertical).normalized;
    }

    // Player Movement
    private void Movement()
    {
        if (_playerStatus._isKnockBack) return;

        _targetVelocity = _moveInput * _playerStatus._moveSpeed;

        // Acceleration & Deceleration Speed Choice
        float moveSpeed = (_targetVelocity.magnitude > _curVelocity.magnitude) ? _playerStatus._accelerationSpeed : _playerStatus._decelerationSpeed;
        _curVelocity = Vector2.MoveTowards(_curVelocity, _targetVelocity, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)_curVelocity * Time.deltaTime;

        Vector2 roundedMove = new Vector2(
            Mathf.Abs(_moveInput.x) < 0.1f ? 0 : Mathf.Sign(_moveInput.x),
            Mathf.Abs(_moveInput.y) < 0.1f ? 0 : Mathf.Sign(_moveInput.y)
        );

        if (_curVelocity.magnitude < 0.01f && _moveInput == Vector2.zero)
        {
            _curVelocity = Vector2.zero;
        }

        if (_moveInput != Vector2.zero)
        {
            _playerAnimator.SetBool("IsMoving", true);
            _playerAnimator.SetBool("IsDeath", false);
            _playerAnimator.SetFloat("MoveX", roundedMove.x);
            _playerAnimator.SetFloat("MoveY", roundedMove.y);
        }
        else
        {
            _playerAnimator.SetBool("IsMoving", false);
            _playerAnimator.SetBool("IsDeath", false);
        }
    }

    // Player Dash Input
    private void DashInput()
    {
        if (_playerStatus._isKnockBack) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_moveInput != Vector2.zero && _dashCoolTime <= 0f )
            {
                _isDash = true;
                _dashProgressTime = _playerStatus._dashDurationTime;
                _dashCoolTime = _playerStatus._dashCoolTime;
                _dashDirection = _moveInput;
            }
        }
        _dashCoolTime -= Time.deltaTime;
    }

    // Player Move Dash
    private void MoveDash()
    {
        if (_playerStatus._isKnockBack) return;

        gameObject.layer = 8;   // Player Layer Change
        transform.position += (Vector3)_dashDirection * _playerStatus._dashSpeed * Time.deltaTime;
        _dashProgressTime -= Time.deltaTime;
        _PlayerSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.7f);

        if (_dashProgressTime <= 0f)
        {
            gameObject.layer = 6;   // Player Layer Change
            _isDash = false;
            _PlayerSprite.color = new Color(1, 1, 1, 1);
        }
    }

    // Player Shoot Input
    private void ShotInput()
    {
        // Direction Zero Setting
        _attackDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _attackDirection += Vector2.up;
            _playerAnimator.Play(UP_ATTACK_HASH);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _attackDirection += Vector2.down;
            _playerAnimator.Play(DOWN_ATTACK_HASH);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _attackDirection += Vector2.left;
            _playerAnimator.Play(LEFT_ATTACK_HASH);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _attackDirection += Vector2.right;
            _playerAnimator.Play(RIGHT_ATTACK_HASH);
        }

        _attackDirection = _attackDirection.normalized;
    }

    private void Attack()
    {
        if (_playerStatus._isKnockBack) return;

        if (_isMeleeWeapon)
        {
            if (_attackDirection != Vector2.zero && _wieldTimer <= 0f)
            {
                GameObject sword = Instantiate(_swordPrefab, transform.position, Quaternion.identity);
                sword.GetComponent<PlayerSwordController>().Init(transform, _attackDirection, _playerStatus._attackSpeed);
                _wieldTimer = _swordAttackDelay / _playerStatus._attackSpeed;   // 공격속도에 비례하여 근접 공격 쿨타임 계산
            }
            _wieldTimer -= Time.deltaTime;
        }
        else
        {
            if (_attackDirection != Vector2.zero && _shotTimer <= 0f)
            {
                GameObject tearGeneration = Instantiate(_tearPrefab, transform.position, Quaternion.identity);
                tearGeneration.GetComponent<Rigidbody2D>().velocity = _attackDirection * _playerStatus._shotSpeed;
                _shotTimer = _tearAttackDelay;
            }
            _shotTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(int damage, Vector2 targetPos)
    {
        if (_isDamaged)
        {
            return;
        }

        _isDamaged = true;
        
        // Player Hp Down
        _playerStatus.HealthDown(damage);

        // Player hit Reaction
        _PlayerSprite.color = new Color(1, 0.4f, 0.1f, 0.7f);

        // Player Layer Change
        gameObject.layer = 8;

        // Player Hit KnockBack
        _playerStatus.PlayerKnockBack(targetPos);
        Invoke("OffDamage", _playerStatus._hitCoolTime);
    }

    private void OffDamage()
    {
        // Player Layer Change
        gameObject.layer = 6;
        _isDamaged = false;
        _PlayerSprite.color = new Color(1, 1, 1, 1);
    }
    
}
