using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkillController : MonoBehaviour
{
    public void FireLasers(LaserSkillSO laserData, Transform caster)
    {
        StartCoroutine(FireLasersCoroutine(laserData, caster));
    }
    
    IEnumerator FireLasersCoroutine(LaserSkillSO laserData, Transform caster)
    {
        List<Transform> nearbyEnemies = FindNearbyEnemies(caster.position, laserData.searchRadius);
        
        if (nearbyEnemies.Count == 0)
        {
            yield break;
        }
        
        for (int i = 0; i < laserData.laserCount; i++)
        {
            nearbyEnemies = FindNearbyEnemies(caster.position, laserData.searchRadius);
            
            if (nearbyEnemies.Count == 0)
            {
                break;
            }
            
            Transform target = nearbyEnemies[Random.Range(0, nearbyEnemies.Count)];
            
            FireSingleLaser(laserData, caster, target);
            
            if (i < laserData.laserCount - 1)
            {
                yield return new WaitForSeconds(laserData.laserDelay);
            }
        }
    }
    
    List<Transform> FindNearbyEnemies(Vector3 center, float radius)
    {
        List<Transform> enemies = new List<Transform>();
        
        MonsterBase[] allMonsters = FindObjectsOfType<MonsterBase>();
        
        foreach (MonsterBase monster in allMonsters)
        {
            if (monster != null && !monster._isDead && monster.gameObject.activeInHierarchy)
            {
                float distance = Vector3.Distance(center, monster.transform.position);
                
                if (distance <= radius)
                {
                    enemies.Add(monster.transform);
                }
            }
        }
        
        return enemies;
    }
    
    void FireSingleLaser(LaserSkillSO laserData, Transform caster, Transform target)
    {
        if (target == null) 
        {
            return;
        }
        
        GameObject laser = Instantiate(laserData.skillPrefab);
        
        if (laser == null)
        {
            return;
        }
        
        laser.transform.position = caster.position;
        
        LaserProjectile laserProjectile = laser.GetComponent<LaserProjectile>();
        if (laserProjectile != null)
        {
            laserProjectile.SetTarget(target);
            laserProjectile.SetDamage(Mathf.RoundToInt(laserData.laserDamage));
        }
    }
}