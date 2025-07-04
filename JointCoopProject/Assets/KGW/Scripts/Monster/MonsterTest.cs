using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour, IDamagable
{
    [SerializeField] int _monsterHp = 100;

    public void TakeDamage(int damage, Vector2 targetPos)
    {
        _monsterHp -= damage;
        Debug.Log($"몬스터 체력 [{_monsterHp}]");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            if(damagable != null)
            {
                damagable.TakeDamage(1, transform.position);
            }
        }
    }
}
