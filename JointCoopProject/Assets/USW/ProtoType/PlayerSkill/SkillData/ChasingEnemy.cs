using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    private GameObject target;
    private float speed;
    private int damage;
    private bool isInit = false;

    public void Init(GameObject targetEnemy, float projectileSpeed, int arrowDamage)
    {
        target = targetEnemy;
        speed = projectileSpeed;
        damage = arrowDamage;
        isInit = true;

        gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        if (!isInit)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        if (target == null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        MonsterBase monster = target.GetComponent<MonsterBase>();
        if (monster != null && monster._isDead)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        Vector3 targetDirection = (target.transform.position - transform.position).normalized;

        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
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
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}