using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonAttackController : MonoBehaviour
{
    int _poisonDamage;
    float _poisonTime = 4.5f;

    public void Init(int _damage)
    {
        _poisonDamage = _damage;
        Destroy(gameObject, _poisonTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if(damagable != null)
            {
                if (!collision.gameObject.GetComponent<PoisonAttackMultiDamage>())
                {
                    PoisonAttackMultiDamage poisonAttackMultiDamage = collision.AddComponent<PoisonAttackMultiDamage>();

                    poisonAttackMultiDamage.Apply(damagable, _poisonDamage);
                }
            }
        }
    }
}
