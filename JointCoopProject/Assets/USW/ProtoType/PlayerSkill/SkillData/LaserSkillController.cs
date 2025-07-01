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
       
        // 4m 반경 내 몬스터들 찾기
        List<Transform> nearbyEnemies = FindNearbyEnemies(caster.position, laserData.searchRadius);
        
        if (nearbyEnemies.Count == 0)
        {
            DebugAllNearbyColliders(caster.position, laserData.searchRadius);
            yield break;
        }
        
        // 3발 연속 발사
        for (int i = 0; i < laserData.laserCount; i++)
        {
            // 랜덤으로 타겟 선택
            Transform target = nearbyEnemies[Random.Range(0, nearbyEnemies.Count)];
            
            // 레이저 발사
            FireSingleLaser(laserData, caster, target);
            
            // 다음 레이저까지 대기
            if (i < laserData.laserCount - 1)
            {
                yield return new WaitForSeconds(laserData.laserDelay);
            }
        }
    }
    
    List<Transform> FindNearbyEnemies(Vector3 center, float radius)
    {
        List<Transform> enemies = new List<Transform>();
        
        if (enemies.Count == 0)
        {
            enemies = FindEnemiesDirectly(center, radius);
        }
        
        return enemies;
    }
    
   
    List<Transform> FindEnemiesDirectly(Vector3 center, float radius)
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
    
    void DebugAllNearbyColliders(Vector3 center, float radius)
    {
        
        // 모든 레이어의 콜라이더 찾기
        Collider2D[] allColliders = Physics2D.OverlapCircleAll(center, radius);
       
        foreach (Collider2D col in allColliders)
        {
            MonsterBase monster = col.GetComponent<MonsterBase>();
        }
    }
    
    // 개별 레이저 발사
    void FireSingleLaser(LaserSkillSO laserData, Transform caster, Transform target)
    {
        if (target == null) 
        {
            return;
        }
        
        // 레이저 프리팹 생성
        GameObject laser = Instantiate(laserData.skillPrefab);
        
        if (laser == null)
        {
            return;
        }
        
        // 레이저 위치를 플레이어 위치로 설정
        laser.transform.position = caster.position;
        
        // 레이저 컨트롤러에 타겟 전달
        LaserProjectile laserProjectile = laser.GetComponent<LaserProjectile>();
        if (laserProjectile != null)
        {
            laserProjectile.SetTarget(target);
        }
    }
}