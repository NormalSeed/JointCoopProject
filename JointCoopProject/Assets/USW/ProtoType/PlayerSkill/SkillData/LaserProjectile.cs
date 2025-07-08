using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [Header("레이저 설정")]
    public float speed = 15f;          
    public float lifeTime = 5f;        
    
    private Transform target;           
    private Vector3 direction;         
    private bool hasTarget = false;
    private int damage;
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
        MoveLaser();
    }

    public void SetDamage(int damageValue)
    {
        damage = damageValue;
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasTarget = true;
        
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
    }
    
    void MoveLaser()
    {
        if (!hasTarget) return;
        
        if (target != null && target.gameObject.activeInHierarchy)
        {
            MonsterBase monster = target.GetComponent<MonsterBase>();
            if (monster != null && !monster._isDead)
            {
                direction = (target.position - transform.position).normalized;
            }
        }
        
        transform.position += direction * speed * Time.deltaTime;
        
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            MonsterBase monster = other.GetComponent<MonsterBase>();
            if (monster != null && monster._isDead)
            {
                return;
            }
            
            damagable.TakeDamage(damage, transform.position);
            
            Destroy(gameObject);
        }
    }
}