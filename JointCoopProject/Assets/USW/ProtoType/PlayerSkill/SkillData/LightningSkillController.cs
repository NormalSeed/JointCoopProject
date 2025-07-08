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
        
        for (int i = 0; i < lightningData.lightningCount; i++)
        {
            nearbyEnemies = FindNearbyEnemies(caster.position, lightningData.searchRadius, lightningData.enemyLayer);
            
            if (nearbyEnemies.Count == 0)
            {
                break;
            }
            
            Transform target = nearbyEnemies[Random.Range(0, nearbyEnemies.Count)];
            
            StrikeSingleLightning(lightningData, target);
            
            if (i < lightningData.lightningCount - 1)
            {
                yield return new WaitForSeconds(lightningData.lightningDelay);
            }
        }
    }
    
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
        lightningPos.y += 1.5f;
        
        GameObject lightning = Instantiate(lightningData.skillPrefab);
        lightning.transform.position = lightningPos;
        
        LightningStrike lightningStrike = lightning.GetComponent<LightningStrike>();
        if (lightningStrike != null)
        {
            lightningStrike.SetDamage(Mathf.RoundToInt(lightningData.lightningDamage));
            lightningStrike.SetEnemyLayer(lightningData.enemyLayer);
        }
    }
}