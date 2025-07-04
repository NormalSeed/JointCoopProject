using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlame : PooledObject
{
    private float _flameSpd;
    public int _damage;
    private Rigidbody2D _rb;
    public SpriteRenderer _flameSR;
    private Animator _animator;
    
    private void Awake() => Init();

    private void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _flameSR = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.Play(0);
    }

    public void Shoot(Vector2 attackDir, float flameSpd, int damage)
    {
        _rb.velocity = Vector2.zero;
        _flameSpd = flameSpd;
        _damage = damage;
        _rb.AddForce(attackDir * _flameSpd, ForceMode2D.Impulse);
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
