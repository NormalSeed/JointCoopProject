using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Player Passive/Poison Attack")]
public class PoisonAttack : SkillDataSO
{
    // 스킬 이름 : 독 공격
    // 단일 타겟
    // 기본 공격 시 독 대미지 부여
    // 20% 확률로 부여 (Level +1 == +20%)
    // 1.5초 마다 5의 대미지 3회 (Level +1 == +2%)
    public int _skillLevel = 1;
    public int _skillDamage = 3;

    public override void UseSkill(Transform caster, Vector3 dir)
    {
        int _totalDamage;
        // Level +1 == 확률 +20%
        skillPossibility *= _skillLevel;

        int _randomValue = Random.Range(0, 100);
        if (_randomValue > skillPossibility)
        {
            return;
        }
        // 독 대미지 (스킬레벨 +1 == 대미지 +2)
        _totalDamage = _skillDamage + (_skillLevel * 2);
        Vector3 poisonSpawnPos = caster.position + dir;
        GameObject poison = Instantiate(skillPrefab, poisonSpawnPos, Quaternion.identity);
        PoisonAttackController poisonAttackController = poison.GetComponent<PoisonAttackController>();
        poisonAttackController.Init(_totalDamage);
    }
}
