using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcRiderBullet : PooledObject
{
    private float _bulletSpd;
    public int _damage;
    private Rigidbody2D _rb;
    public SpriteRenderer _bulletSR;
    private Animator _animator;

    private void Awake() => Init();

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bulletSR = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.Play(0);
    }

    public void Shoot(Vector2 attackDir, float bulletSpd, int damage)
    {
        _rb.velocity = Vector2.zero;
        _bulletSpd = bulletSpd;
        _damage = damage;
        _rb.AddForce(attackDir * _bulletSpd, ForceMode2D.Impulse);
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
