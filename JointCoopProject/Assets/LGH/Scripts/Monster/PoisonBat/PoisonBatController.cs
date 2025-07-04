using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PoisonBatController : MonsterBase
{
    private Coroutine _coDotDeal;
    private WaitForSeconds dotDelay = new WaitForSeconds(1.5f);
    protected override void Init()
    {
        base.Init();
        _monsterID = 10103;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(_model._bodyDamage, transform.position);
                _coDotDeal = StartCoroutine(CoDotDeal(damagable));
            }
        }
    }

    private IEnumerator CoDotDeal(IDamagable target)
    {
        yield return dotDelay;

        if (target != null)
        {
            target.TakeDamage(_model._bodyDamage, transform.position);
        }
    }
}
