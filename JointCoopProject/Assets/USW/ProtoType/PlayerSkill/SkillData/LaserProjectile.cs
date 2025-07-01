using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [Header("레이저 설정")]
    public float speed = 15f;          
    public int damage = 10;             
    public float lifeTime = 5f;        
    
    private Transform target;           
    private Vector3 direction;         
    private bool hasTarget = false;    
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
        MoveLaser();
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasTarget = true;
        
        if (target != null)
        {
            // 타겟 방향 계산
            direction = (target.position - transform.position).normalized;
        }
    }
    
    void MoveLaser()
    {
        if (!hasTarget) return;
        
        // 타겟이 살아있으면 계속 추적
        if (target != null && target.gameObject.activeInHierarchy)
        {
            // 실시간으로 방향 업데이트 (추적 레이저)
            direction = (target.position - transform.position).normalized;
        }
        
        // 레이저 이동
        transform.position += direction * speed * Time.deltaTime;
        
        // 레이저 회전 (방향에 맞게)
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 몬스터와 충돌 시
        MonsterBase monster = other.GetComponent<MonsterBase>();
        if (monster != null && !monster._isDead)
        {
            // 데미지 적용
            monster.TakeDamage(damage, transform.position);
            
            // 레이저 제거
            Destroy(gameObject);
        }
    }
}