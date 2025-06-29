using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow: PooledObject
{
    private float _arrowSpd;
    private Rigidbody2D _rb;
    public SpriteRenderer _arrowSR;
    public int _damage;

    private void Awake() => Init();

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _arrowSR = GetComponent<SpriteRenderer>();
    }

    public void Shoot(Vector2 attackDir, float arrowSpd, int damage)
    {
        _rb.velocity = Vector2.zero;
        _arrowSpd = arrowSpd;
        _damage = damage;
        _rb.AddForce(attackDir * _arrowSpd, ForceMode2D.Impulse);
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
