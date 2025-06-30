using System.Collections;
using System.Collections.Generic;
using System.Data;
using TreeEditor;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    Transform _playerPos;

    Vector2 _wieldDirection;        // 휘두르는 방향 벡터
    float _wieldAngle = 90f;       // 무기의 휘두름 반경
    float _rotationSpeed = 420f;    // 회전 속도 (값이 높을 수록 빨리 회전한다)
    float _wieldRadius = 0.3f;      // 무기 회전 반경
    float _currentAngle = 0;       // 현재 각도
    float _startAngle = 0;         // 시작 각도
    float _wieldSpeed;              // 플레이어의 공격 속도
    int _attackDamage;

    
    private void Update()
    {
        AttackAngle();
    }

    // 데이터 초기화
    public void Init(Transform player, Vector2 direction, float attackSpeed, int attackDamage)
    {
        _playerPos = player;
        _wieldDirection = direction.normalized;
        _wieldSpeed = attackSpeed;
        _currentAngle = 0f;
        _attackDamage = attackDamage;

        _startAngle = Mathf.Atan2(_wieldDirection.y, _wieldDirection.x) * Mathf.Rad2Deg;

        float radian = _startAngle * Mathf.Deg2Rad;
        Vector3 firstOffset = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * _wieldRadius;
        transform.position = _playerPos.position + firstOffset;

        transform.rotation = Quaternion.Euler(0, 0, _currentAngle + _startAngle + 90f);
    }

    // 검의 휘두름 각도 세팅
    private void AttackAngle()
    {
        float angle = _rotationSpeed * _wieldSpeed * Time.deltaTime;    // 회전스피드가 플레이어의 공격스피드에 영향을 받아 빨리 휘두름
        _currentAngle -= angle;

        // 현재 각도로 위치를 플레이어의 중심을 기준으로 재계산
        float radian = (_currentAngle + _startAngle) * Mathf.Deg2Rad;
        Vector3 posOffset = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * _wieldRadius;   // 현재의 각도와 시작각도를 더한 값에 회전 반경을 곱해서 위치 옵셋 세팅
        transform.position = _playerPos.position + posOffset;

        transform.rotation = Quaternion.Euler(0, 0, _currentAngle + _startAngle + 90f);

        if (_currentAngle <= -_wieldAngle)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log($"적을 {_attackDamage}로 공격했습니다.");
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_attackDamage, transform.position);
            }
        }
    }

    
}
