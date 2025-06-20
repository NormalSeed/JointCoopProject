using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    IsaacStatus _isaacStatus;
    Rigidbody2D _isaacRigid;
    Vector2 _moveInput;
    Vector2 _targetVelocity;
    Vector2 _curVelocity;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        MoveInput();
        Movement();
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
        float moveSpeed = (_targetVelocity.magnitude > _curVelocity.magnitude) ? _isaacStatus._accelerationSpeed : _isaacStatus._DecelerationSpeed;
        _curVelocity = Vector2.MoveTowards(_curVelocity, _targetVelocity, moveSpeed * Time.deltaTime);
        transform.position += (Vector3)(_curVelocity * Time.deltaTime);        
    }
}
