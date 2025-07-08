using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [Header("번개 설정")]
    public float strikeRadius = 1f;
    public float duration = 1f;

    private int damage;
    private LayerMask enemyLayer;
    private bool hasStruck = false;
    
    void Start()
    {
        Destroy(gameObject, duration);
        Invoke("StrikeEnemies", 0.1f);
    }
    
    public void SetDamage(int damageValue)
    {
        damage = damageValue;
    }
    
    public void SetEnemyLayer(LayerMask layer)
    {
        enemyLayer = layer;
    }
    
    void StrikeEnemies()
    {
        if (hasStruck) return;
        hasStruck = true;
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, strikeRadius, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            IDamagable damagable = enemy.GetComponent<IDamagable>();
            if (damagable != null)
            {
                MonsterBase monster = enemy.GetComponent<MonsterBase>();
                if (monster != null && monster._isDead)
                {
                    continue;
                }
                
                damagable.TakeDamage(damage, transform.position);
            }
        }
    }
}