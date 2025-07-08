using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackController : MonoBehaviour
{
    public float _dashDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable damagableComponent = collision.GetComponent<IDamagable>();
        if (damagableComponent != null && collision.CompareTag("Enemy"))
        {
            Debug.Log("take Damage to Enemy!");
            damagableComponent.TakeDamage((int)_dashDamage, transform.position);
        }
    }
}
