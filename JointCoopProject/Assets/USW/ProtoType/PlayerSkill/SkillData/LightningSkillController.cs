using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkillController : MonoBehaviour
{
    public void StrikeLightning(LightningSkillSO lightningData, Transform caster)
    {
        StartCoroutine(StrikeLightningCoroutine(lightningData, caster));
    }
    
    IEnumerator StrikeLightningCoroutine(LightningSkillSO lightningData, Transform caster)
    {
        List<Transform> nearbyEnemies = FindNearbyEnemies(caster.position, lightningData.searchRadius, lightningData.enemyLayer);
        
        if (nearbyEnemies.Count == 0)
        {
            yield break;
        }
        
        
        // 3개 번개 연속으로 떨어뜨리기
        for (int i = 0; i < lightningData.lightningCount; i++)
        {
            // 랜덤으로 타겟 선택
            Transform target = nearbyEnemies[Random.Range(0, nearbyEnemies.Count)];
            
            // 번개 떨어뜨리기
            StrikeSingleLightning(lightningData, target);
            
            
            // 다음 번개까지 대기
            if (i < lightningData.lightningCount - 1)
            {
                yield return new WaitForSeconds(lightningData.lightningDelay);
            }
        }
    }
    
    // 근처 적들 찾기
    List<Transform> FindNearbyEnemies(Vector3 center, float radius, LayerMask enemyLayer)
    {
        List<Transform> enemies = new List<Transform>();
        
     
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius, enemyLayer);
        
        foreach (Collider2D col in colliders)
        {
            MonsterBase monster = col.GetComponent<MonsterBase>();
            if (monster != null && !monster._isDead && monster.gameObject.activeInHierarchy)
            {
                enemies.Add(col.transform);
            }
        }
        
 
        
        return enemies;
    }
    
  
    void StrikeSingleLightning(LightningSkillSO lightningData, Transform target)
    {
        if (target == null) return;
        
        
        Vector3 lightningPos = target.position;
        // 피벗 설정하기가 더 귀차낭
        lightningPos.y += 1.5f;
     
        GameObject lightning = Instantiate(lightningData.skillPrefab);
        lightning.transform.position = lightningPos;
    }
}