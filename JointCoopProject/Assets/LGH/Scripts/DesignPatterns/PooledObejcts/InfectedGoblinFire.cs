using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedGoblinFire : PooledObject
{
    private float _fireSpd;
    private Rigidbody2D _rb;
    public SpriteRenderer _fireSR;
    public int _damage;

    private void Awake() => Init();

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _fireSR = GetComponent<SpriteRenderer>();
    }

    public void Shoot(Vector2 attackDir, float arrowSpd, int damage)
    {
        _rb.velocity = Vector2.zero;
        _fireSpd = arrowSpd;
        _damage = damage;
        _rb.AddForce(attackDir * _fireSpd, ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_damage, transform.position);
                Debug.Log("Àû¿¡°Ô ¸ÂÃã");
                ReturnPool();
            }
        }
        else if (collision != null)
        {
            ReturnPool();
        }
    }
}
