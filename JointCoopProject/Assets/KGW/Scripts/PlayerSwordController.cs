using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    Transform _playerPos;

    float _wieldDuration = 0.3f;
    float _timer = 0;
    int _swordDamage = 1;
    Vector2 _wieldDirection;
    float _wieldAngle = 120f;
    float _rotationSpeed = 420f;
    float _wieldRadius = 0.5f;
    float _currentAngle = 0f;
    float _startAngle = 0f;
    float _wieldSpeed;

    private void Update()
    {
        if (_playerPos == null)
        {
            Destroy(gameObject);
            return;
        }

        //_timer -= Time.deltaTime;

        float angle = _rotationSpeed * _wieldSpeed * Time.deltaTime;
        _currentAngle += angle;
   
        // 현재 각도로 위치를 플레이어의 중심을 기준으로 재계산
        float radian = (_currentAngle + _startAngle) * Mathf.Deg2Rad;
        Vector3 posOffset = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * _wieldRadius;
        transform.position = _playerPos.position + posOffset;

        transform.rotation = Quaternion.Euler(0, 0, _currentAngle + _startAngle - 80f);

        if (_currentAngle >= _wieldAngle)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(collision.GetComponent<IDamagable>() != null)
            {
                // 대상이 공격을 받을 수 있으므로 데미지를 준다.
            }
        }
    }

    public void Init(Transform player, Vector2 direction, float attackSpeed)
    {
        _playerPos = player;
        _wieldDirection = direction.normalized;
        _timer = _wieldDuration;
        _wieldSpeed = attackSpeed;
        _currentAngle = 0f;

        _startAngle = Mathf.Atan2(_wieldDirection.y, _wieldDirection.x) * Mathf.Rad2Deg;

        float radian = _startAngle * Mathf.Deg2Rad;
        Vector3 firstOffset = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * _wieldRadius;
        transform.position = _playerPos.position + firstOffset;

        transform.rotation = Quaternion.Euler(0, 0, _currentAngle + _startAngle - 80f);

    }
}
