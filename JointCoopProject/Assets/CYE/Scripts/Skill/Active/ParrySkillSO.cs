using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Active/Parry")]
public class ParrySkillSO : SkillDataSO
{
    [SerializeField] private float _skillDamageRate; // 대미지 배율
    [SerializeField] private float _skillRange; // 사거리
    // float _skillCoolTime; // SkillDataSO에 있음
    [SerializeField] private float _duration; // 지속시간
    [SerializeField] private float _HealthRecovery; // 회복량
    [SerializeField] private int _cost; // 소모량

    public override void UseSkill(Transform caster)
    {
        // 플레이어 반격 상태
        PlayerStatManager.Instance._isParry = true;
    }
    public override void ReleaseSkill()
    {
        PlayerStatManager.Instance._isParry = false;
        // PlayerStatManager.Instance._attackSpeed = _prevAttackSpeed;
    }
}
