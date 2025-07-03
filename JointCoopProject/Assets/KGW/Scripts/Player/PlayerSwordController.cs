using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    Transform _playerPos;   // 플레이어 위치
    Vector2 _attackDirection;   // 공격 방향
    float _attackSpeed; // 공격 속도
    float _attackRange; // 공격 사거리
    int _attackDamage;  // 공격 대미지
    PlayerSkillManager _skillManager;

    // 데이터 초기화
    public void Init(Transform player, Vector2 direction, float attackSpeed, int attackDamage, PlayerSkillManager skillManager, float attackRange)
    {
        _playerPos = player;
        _attackDirection = direction.normalized;
        _attackSpeed = attackSpeed;
        _attackDamage = attackDamage;
        _skillManager = skillManager;
        _attackRange = attackRange;

        float SwordTimer = 0.3f / Mathf.Clamp(_attackSpeed, 0.01f, 10f); // 공격속도에 반비례하여 유지시간 감소(공속 2 → 0.5초 유지)
        Destroy(gameObject, SwordTimer);
    }

    private void Update()
    {
        // 플레이어의 위치를 따라가도록
        transform.position = _playerPos.position + (Vector3)(_attackDirection * _attackRange);

        float angle = Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
                _skillManager.SwordUpgradeAttack();
            }
        }
    }    
}
