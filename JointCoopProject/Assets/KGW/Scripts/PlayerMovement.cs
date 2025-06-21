using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject _tearPrefab;
    //[SerializeField] float _attackDelay = 0.3f;

    PlayerStatus _playerStatus;
    Rigidbody2D _playerRigid;

    Vector2 _moveInput;
    Vector2 _targetVelocity;
    Vector2 _curVelocity;
    Vector2 _shotDirection;
    Vector2 _dashDirection;

    float _shotTimer;
    bool _isDash = false;
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
        }
        else
        {
            MoveDash();
        }
        
           
        //ShotTear();
    }

    // First Initialize
    private void Init()
    {
        _playerRigid = GetComponent<Rigidbody2D>();
        _playerStatus = GetComponent<PlayerStatus>();
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
        _shotDirection = Vector2.zero;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            _shotDirection += Vector2.up;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            _shotDirection += Vector2.down;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            _shotDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _shotDirection += Vector2.right;
        }

        _shotDirection = _shotDirection.normalized;
    }

    // TODO : 플레이어의 눈물공격은 일단 보류
    // Player Shoot Tear
    //private void ShotTear()
    //{
    //    if(_shotDirection != Vector2.zero && _shotTimer <= 0f)
    //    {
    //        GameObject tearGeneration = Instantiate(_tearPrefab, transform.position, Quaternion.identity);
    //        tearGeneration.GetComponent<Rigidbody2D>().velocity = _shotDirection * _isaacStatus._shotSpeed;
    //        _shotTimer = _attackDelay;
    //    }
    //    _shotTimer -= Time.deltaTime;
    //}
}
