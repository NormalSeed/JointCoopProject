using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Passive/SpinArrow")]
public class SpinArrow : SkillDataSO
{
    // 단일타겟
    // 범위내 몬스터 추적
    // 사거리 없을시 캐릭터 시선으로 발사 
    // 관통 없음 ( 즉시 파괴 )
    // 3초 stunDuration
    public float detectionRange = 4f;
    public float projectileSpeed;
    public string enemyTag = "Enemy";



    public override void UseSkill(Transform caster)
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue > skillPossibility)
        {
            return;
        }

        // 주변 적 탐지

        //GameObject target
    }

    private GameObject FindClosestEnemy(Vector3 casterPosition)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closestEnemy = null;
        float closestDistance = detectionRange;

        foreach (GameObject enemy in enemies)

        {
            float distance = Vector3.Distance(casterPosition, enemy.transform.position);
            if (distance <= detectionRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;

    }
    // 
}
