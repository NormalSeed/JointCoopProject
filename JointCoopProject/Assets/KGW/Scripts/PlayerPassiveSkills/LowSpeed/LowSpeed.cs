using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Player Passive/Low Speed")]
public class LowSpeed : SkillDataSO
{
    // 스킬 이름 : 이동속도 저하
    // 단일 타겟
    // 기본 공격 시 몬스터 이동속도 저하 부여
    // 20% 확률로 부여 (Level +1 == +10%)
    // 10% 이동속도 저하 (Level +1 == +10%)
    public int _skillLevel = 1;

    public override void UseSkill(Transform caster, Vector3 dir)
    {
        // Level +1 == 확률 +20%
        skillPossibility *= _skillLevel;

        int _randomValue = Random.Range(0, 100);
        if (_randomValue > skillPossibility)
        {
            return;
        }
        Vector3 lowSpeedSpawnPos = caster.position + dir;
        GameObject lowSpeed = Instantiate(skillPrefab, lowSpeedSpawnPos, Quaternion.identity);
    }
}
