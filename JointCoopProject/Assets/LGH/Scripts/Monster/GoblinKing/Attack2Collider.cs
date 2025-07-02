using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Collider : MonoBehaviour
{
    private GoblinKingModel _model;

    private void Awake() => Init();

    private void Init()
    {
        _model = GetComponentInParent<GoblinKingModel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_model._attack2Damage, transform.position);
            }
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce((collision.gameObject.transform.position - transform.position).normalized * 20f, ForceMode2D.Impulse);
            }
        }
    }
}
