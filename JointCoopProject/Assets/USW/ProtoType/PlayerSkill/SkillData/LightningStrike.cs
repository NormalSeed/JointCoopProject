using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    [Header("번개 설정")]
    public int damage = 15;            
    public float strikeRadius = 1f;      
    public float duration = 1f;
    public LayerMask enemyLayer;  
    
    private bool hasStruck = false;     
    
    void Start()
    {
        // 번개 수명 설정
        Destroy(gameObject, duration);
    }
    
    void StrikeEnemies()
    {
        if (hasStruck) return;
        hasStruck = true;
        
        
        // 번개 범위 내 모든 적들에게 데미지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, strikeRadius, enemyLayer);
        
        int hitCount = 0;
        foreach (Collider2D enemy in hitEnemies)
        {
            MonsterBase monster = enemy.GetComponent<MonsterBase>();
            if (monster != null && !monster._isDead)
            {
                monster.TakeDamage(damage, transform.position);
                hitCount++;
            }
        }
        
    }
    
}