using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject _tearPrefab;
    [SerializeField] float _shotDelay = 0.3f;

    IsaacStatus _isaacStatus;
    Rigidbody2D _isaacRigid;
    float _shotTimer;
    Vector2 _moveInput;
    Vector2 _targetVelocity;
    Vector2 _curVelocity;
    Vector2 _shotDirection;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        MoveInput();
        Movement();
        ShotInput();
        ShotTear();
    }

    // First Initialize
    private void Init()
    {
        _isaacRigid = GetComponent<Rigidbody2D>();
        _isaacStatus = GetComponent<IsaacStatus>();
    }

    // Isaac Move Input
    private void MoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _moveInput = new Vector2(horizontal, vertical).normalized;
    }

    // Isaac Movement
    private void Movement()
    {
        _targetVelocity = _moveInput * _isaacStatus._moveSpeed;

        // Acceleration & Deceleration Speed Choice
        float moveSpeed = (_targetVelocity.magnitude > _curVelocity.magnitude) ? _isaacStatus._accelerationSpeed : _isaacStatus._decelerationSpeed;
        _curVelocity = Vector2.MoveTowards(_curVelocity, _targetVelocity, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)(_curVelocity * Time.deltaTime);        
    }

    // Isaac Shoot Input
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

    // Isaac Shoot Tear
    private void ShotTear()
    {
        if(_shotDirection != Vector2.zero && _shotTimer <= 0f)
        {
            GameObject tearGeneration = Instantiate(_tearPrefab, transform.position, Quaternion.identity);
            tearGeneration.GetComponent<Rigidbody2D>().velocity = _shotDirection * _isaacStatus._shotSpeed;
            _shotTimer = _shotDelay;
        }
        _shotTimer -= Time.deltaTime;
    }
}
