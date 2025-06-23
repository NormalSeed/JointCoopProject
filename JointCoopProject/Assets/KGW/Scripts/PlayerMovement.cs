using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Weapon Status")]
    [SerializeField] GameObject _tearPrefab;
    [SerializeField] GameObject _swordPrefab;
    [SerializeField] float _tearAttackDelay = 0.3f;
    [SerializeField] float _swordAttackDelay = 0.5f;

    PlayerStatus _playerStatus;
    PlayerTearController _tearController;
    PlayerSwordController _swordController;
    Rigidbody2D _playerRigid;

    Vector2 _moveInput;
    Vector2 _targetVelocity;
    Vector2 _curVelocity;
    Vector2 _attackDirection;
    Vector2 _dashDirection;

    float _shotTimer;
    float _wieldTimer;
    bool _isDash = false;
    public bool _isMeleeWeapon = true;
    float _dashProgressTime;
    float _dashCoolTime;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        MoveInput();

        if (!_isDash)
        {
            Movement();
            ShotInput();
            DashInput();
            Attack();
        }
        else
        {
            MoveDash();
        }
    }

    // First Initialize
    private void Init()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerStatus = GetComponent<PlayerStatus>();
        _tearController = GetComponent<PlayerTearController>();
        _swordController = GetComponent<PlayerSwordController>();
    }

    // Player Move Input
    private void MoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _moveInput = new Vector2(horizontal, vertical).normalized;
    }

    // Player Movement
    private void Movement()
    {
        _targetVelocity = _moveInput * _playerStatus._moveSpeed;

        // Acceleration & Deceleration Speed Choice
        float moveSpeed = (_targetVelocity.magnitude > _curVelocity.magnitude) ? _playerStatus._accelerationSpeed : _playerStatus._decelerationSpeed;
        _curVelocity = Vector2.MoveTowards(_curVelocity, _targetVelocity, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)_curVelocity * Time.deltaTime;
        
        if(_curVelocity.magnitude < 0.01f && _moveInput == Vector2.zero)
        {
            _curVelocity = Vector2.zero;
        }

    }

    // Player Dash Input
    private void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
        transform.position += (Vector3)_dashDirection * _playerStatus._dashSpeed * Time.deltaTime;
        _dashProgressTime -= Time.deltaTime;

        if (_dashProgressTime <= 0f)
        {
            _isDash = false;
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
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _attackDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _attackDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _attackDirection += Vector2.right;
        }

        _attackDirection = _attackDirection.normalized;
    }

    private void Attack()
    {
        if(_isMeleeWeapon)
        {
            if (_attackDirection != Vector2.zero && _wieldTimer <= 0f)
            {
                //Quaternion swordRotation = Quaternion.FromToRotation(Vector3.right, _attackDirection);
                GameObject sword = Instantiate(_swordPrefab, transform.position, Quaternion.identity);
                sword.GetComponent<PlayerSwordController>().Init(transform, _attackDirection, _playerStatus._attackSpeed);
                _wieldTimer = _swordAttackDelay / _playerStatus._attackSpeed;
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
}
