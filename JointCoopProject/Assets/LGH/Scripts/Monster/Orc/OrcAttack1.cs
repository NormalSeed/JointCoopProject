using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcAttack1 : MonoBehaviour
{
    private MonsterModel _model;

    private void Awake() => Init();

    private void Init()
    {
        _model = GetComponentInParent<MonsterModel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_model._attack1Damage, transform.position);
            }
        }
    }
}
