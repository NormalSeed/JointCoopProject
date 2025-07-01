using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour, IDamagable
{
    [SerializeField] float _knockBackForce = 1.5f;
    [SerializeField] float _knockBackTime = 0.5f;
    Rigidbody2D _monsterRigid;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _monsterRigid = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Vector2 targetPos)
    {
        Vector2 hitDirection = ((Vector2)transform.position - targetPos).normalized;

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
