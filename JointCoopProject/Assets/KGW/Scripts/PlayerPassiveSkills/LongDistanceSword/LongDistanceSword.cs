using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Player Passive/Long Distance Sword")]
public class LongDistanceSword : SkillDataSO
{
    // 스킬 이름 : 검기 
    // 단일 타겟
    // 기본 공격 시 확률로 발동
    // 10% 확률로 발동 (Level +1 == +10%)
    // 20의 대미지 (4Level +10, 5Level + 20)
    public int _skillLevel = 1;
    public int _skillDamage = 20;
    public float _projectileSpeed = 5f; // 투사체 속도

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
        // 검기 대미지 (1~3Level 기존 대미지, 4Level +10, 5Level + 20)
        _totalDamage = _skillLevel == 4 ? 30 : _skillLevel == 5 ? 50 : _skillDamage;

        // 검기 생성 위치를 공격 이펙트에서 생성
        Vector3 swordEnergyPos = caster.transform.position;
        swordEnergyPos += dir;

        GameObject swordEnergyThrow = Instantiate(skillPrefab, swordEnergyPos, caster.rotation);
        LongDistanceSwordController poisonAttackController = swordEnergyThrow.GetComponent<LongDistanceSwordController>();
        poisonAttackController.Init(dir, _projectileSpeed, _totalDamage);

        // 캐릭터의 이동방향에 맞게 단검 회전
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        poisonAttackController.transform.rotation = Quaternion.Euler(0f, 0f, angle);

    }
}
