using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Passive/SpinArrow")]
public class SpinArrow : SkillDataSO
{
    [Header("화살 설정")]
    public float detectionRange = 4f;
    public float projectileSpeed = 10f;
    public float arrowDamage = 15f;
    public string enemyTag = "Enemy";

    public override void UseSkill(Transform caster)
    {
        float randomValue = Random.Range(0f, 100f);
        if (randomValue > skillPossibility)
        {
            return;
        }

        GameObject target = FindClosestEnemy(caster.position);
        if (target == null)
        {
            return;
        }

        GameObject arrow = Instantiate(skillPrefab);
        if (arrow == null)
        {
            return;
        }

        arrow.transform.position = caster.position;

        ChasingEnemy chasingEnemy = arrow.GetComponent<ChasingEnemy>();
        if (chasingEnemy != null)
        {
            chasingEnemy.Init(target, projectileSpeed, Mathf.RoundToInt(arrowDamage));
        }
    }

    private GameObject FindClosestEnemy(Vector3 casterPosition)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closestEnemy = null;
        float closestDistance = detectionRange;

        foreach (GameObject enemy in enemies)
        {
            MonsterBase monster = enemy.GetComponent<MonsterBase>();
            if (monster != null && monster._isDead)
            {
                continue;
            }

            float distance = Vector3.Distance(casterPosition, enemy.transform.position);
            if (distance <= detectionRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
}